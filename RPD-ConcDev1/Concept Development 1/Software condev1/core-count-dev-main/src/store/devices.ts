import { Capacitor } from '@capacitor/core';
import { defineStore } from 'pinia';
import {
	BleClient,
	BleDevice,
	BleService,
	RequestBleDeviceOptions,
	ScanResult,
	BleCharacteristic,
} from '@capacitor-community/bluetooth-le';
import { ref } from 'vue';
import { useLogStore } from './log';

const DATA_SERVICE_UUID = '12345678-1234-5678-1234-56789abcdef0';
const MMG_DATA_CHARACTERISTIC_UUID = '12345678-1234-5678-1234-56789abcdef1';
const IMU_DATA_CHARACTERISTIC_UUID = '12345678-1234-5678-1234-56789abcdef2';



export const useDevicesStore = defineStore('devices', () => {
	const { log } = useLogStore();
	const initialized = ref(false);
	const connectingToDevice = ref(false);
	let connectivityCheckerTimeout: number | undefined;
	const deviceList = ref<BleDevice[]>([]);
	const connectedDevices = ref<BleDevice[]>([]);
	const deviceServices = ref<{
		[deviceId: string]: BleService[];
	}>({});
	const activeNotifications = ref<{
		[deviceId: string]: {
			[serviceId: string]: {
				[characteristicId: string]: boolean;
			};
		};
	}>({});

	const storeDevice = (device: BleDevice) => {
		if (deviceList.value.find((d) => d.deviceId === device.deviceId)) {
			return;
		}
		deviceList.value.push(device);
	};

	const storeConnectedDevice = (device: BleDevice) => {
		if (connectedDevices.value.find((d) => d.deviceId === device.deviceId)) {
			return;
		}
		connectedDevices.value.push(device);
	};

	const storeDeviceServices = (deviceId: string, services: BleService[]) => {
		deviceServices.value[deviceId] = services;
	};

	const initialize = async () => {
		log('Initializing...');
		// Check if location is enabled
		if (Capacitor.getPlatform() === 'android') {
			log('Checking location...');
			const isLocationEnabled = await BleClient.isLocationEnabled();
			if (!isLocationEnabled) {
				await BleClient.openLocationSettings();
			}
		}
		log('Initializing Bluetooth...');
		await BleClient.initialize();
		initialized.value = true;
	};

	const scanForDevices = async () => {
		await BleClient.requestLEScan(
			{
				services: [DATA_SERVICE_UUID],
			},
			(result: ScanResult) => {
				storeDevice(result.device);
				log(
					`Found device: ${result.device.name} (${result.device.deviceId})`
				);
			}
		);
	};

	const getServices = async (device: BleDevice): Promise<BleService[]> => {
		log(`Getting services for ${device.name} (${device.deviceId})...`);
		try {
			const services = await BleClient.getServices(device.deviceId);
			log(`Got ${services.length} services for ${device.name} (${device.deviceId})`);
			storeDeviceServices(device.deviceId, services);
			return services;
		} catch (error) {
			log((<Error>error).message, 'error');
			return [];
		}
	};

	const connectToDevice = async (device: BleDevice) => {
		log(`Connecting to ${device.name} (${device.deviceId})...`);
		connectingToDevice.value = true;
		try {
			// Try to disconnect first in case we're already connected
			await BleClient.disconnect(device.deviceId);
			await BleClient.connect(device.deviceId, () => {
				log(`Disconnected from ${device.name} (${device.deviceId})`);
				connectedDevices.value = connectedDevices.value.filter(
					(d) => d.deviceId !== device.deviceId
				);
			});
			storeConnectedDevice(device);
			log(`Connected to ${device.name} (${device.deviceId})`);

			await getServices(device);

			// setInterval(async () => {
			// 	log(`Reading MMG`);
			// 	await read(device, DATA_SERVICE_UUID, MMG_DATA_CHARACTERISTIC_UUID);
			// 	log(`Reading IMU`);
			// 	await read(device, DATA_SERVICE_UUID, IMU_DATA_CHARACTERISTIC_UUID);
			// 	await startNotifications(device);
			// }, 3000);
		} catch (error) {
			log((<Error>error).message, 'error');
		} finally {
			connectingToDevice.value = false;
		}

	};

	const disconnectFromDevice = async (device: BleDevice) => {
		log(`Disconnecting from ${device.name} (${device.deviceId})...`);
		try {
			await BleClient.disconnect(device.deviceId);
			connectedDevices.value = connectedDevices.value.filter(
				(d) => d.deviceId !== device.deviceId
			);
			//log(`Disconnected from ${device.name} (${device.deviceId})`);
		} catch (error) {
			log((<Error>error).message, 'error');
		}
	};

	const checkConnectedDevices = async (withServices: string[]) => {
		//log(`Checking connected devices...`);
		try {
			const devices = await BleClient.getConnectedDevices(withServices);
			//log(`Got connected devices`);
			connectedDevices.value = devices;
		} catch (error) {
			log((<Error>error).message, 'error');
		}
	};

	const readCharacteristic = async (device: BleDevice, service: BleService, characteristic: BleCharacteristic) => {
		try {
			log(`Reading ${characteristic.uuid} from ${device.name} (${device.deviceId})...`);
			const result = await BleClient.read(device.deviceId, service.uuid, characteristic.uuid);
			console.log(result);
			log(`Result: ${result.getUint8(0)}`);
		} catch (error) {
			log((<Error>error).message, 'error');
		}
	};

	const startNotifications = async (device: BleDevice, service: BleService, characteristic: BleCharacteristic, callback: (value: any) => void) => {
		try {
			log(`Starting notifications for ${characteristic.uuid} from ${device.name} (${device.deviceId})...`);
			await BleClient.startNotifications(device.deviceId, service.uuid, characteristic.uuid, callback);
			if(!activeNotifications.value[device.deviceId]) {
				activeNotifications.value[device.deviceId] = {};
			}
			if(!activeNotifications.value[device.deviceId][service.uuid]) {
				activeNotifications.value[device.deviceId][service.uuid] = {};
			}
			activeNotifications.value[device.deviceId][service.uuid][characteristic.uuid] = true;
		} catch (error) {
			log((<Error>error).message, 'error');
		}
	};

	const stopNotifications = async (device: BleDevice, service: BleService, characteristic: BleCharacteristic) => {
		try {
			log(`Stopping notifications for ${characteristic.uuid} from ${device.name} (${device.deviceId})...`);
			await BleClient.stopNotifications(device.deviceId, service.uuid, characteristic.uuid);
			activeNotifications.value[device.deviceId][service.uuid][characteristic.uuid] = false;
		} catch (error) {
			log((<Error>error).message, 'error');
		}
	}

	const connectivityChecker = async () => {
		if(initialized.value && !connectingToDevice.value) {
			await checkConnectedDevices([DATA_SERVICE_UUID]);
		}

		if (connectivityCheckerTimeout) {
			clearTimeout(connectivityCheckerTimeout);
		}

		connectivityCheckerTimeout = setTimeout(connectivityChecker, 1000);
	};

	//connectivityChecker();

	return {
		initialize,
		scanForDevices,
		deviceList,
		getServices,
		deviceServices,
		connectedDevices,
		connectToDevice,
		disconnectFromDevice,
		checkConnectedDevices,
		readCharacteristic,
		startNotifications,
		stopNotifications,
	};
});
