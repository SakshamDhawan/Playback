import { Capacitor } from '@capacitor/core';
import {
	BleClient,
	BleDevice,
	BleService,
	RequestBleDeviceOptions,
	ScanResult,
} from '@capacitor-community/bluetooth-le';

function log(line: string) {
	console.log(line);
}

export default function useBLE() {
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
		await scan();
	};

	const scan = async (scanOptions: RequestBleDeviceOptions) => {
		log('Scanning...');
		await BleClient.requestLEScan(
			scanOptions,
			(result: ScanResult) => {
				foundDevices.value.push(result.device);
				log(
					`Found device: ${result.device.name} (${result.device.deviceId})`
				);
			}
		);

	};

	const connectToDevice = async (device: BleDevice) => {
		log(`Connecting to ${device.name} (${device.deviceId})...`);
		try {
			// Try to disconnct first in case we're already connected
			await BleClient.disconnect(device.deviceId);
			await BleClient.connect(device.deviceId);
			connectedDevices.value.push(device);
			log(`Connected to ${device.name} (${device.deviceId})`);
			//await getServices(device);
			// setInterval(async () => {
			// 	log(`Reading MMG`);
			// 	await read(device, DATA_SERVICE_UUID, MMG_DATA_CHARACTERISTIC_UUID);
			// 	log(`Reading IMU`);
			// 	await read(device, DATA_SERVICE_UUID, IMU_DATA_CHARACTERISTIC_UUID);
			// 	await startNotifications(device);
			// }, 3000);
		} catch (error) {
			log((<Error>error).message);
		}
	};

	const read = async (
		device: BleDevice,
		serviceUUID: string,
		characteristicUUID: string
	) => {
		log(`Reading from ${device.name} (${device.deviceId})...`);
		try {
			const result = await BleClient.read(
				device.deviceId,
				serviceUUID,
				characteristicUUID
			);
			log(`Read result: ${result}`);
			console.log(result);
		} catch (error) {
			log((<Error>error).message);
		}
	};

	const startNotifications = async (device: BleDevice) => {
		log(
			`Starting notifications for ${device.name} (${device.deviceId})...`
		);
		try {
			await BleClient.startNotifications(
				device.deviceId,
				DATA_SERVICE_UUID,
				MMG_DATA_CHARACTERISTIC_UUID,
				(result) => {
					log(`MMG data: ${result}`);
				}
			);
			await BleClient.startNotifications(
				device.deviceId,
				DATA_SERVICE_UUID,
				IMU_DATA_CHARACTERISTIC_UUID,
				(result) => {
					log(`IMU data: ${result}`);
				}
			);
			log(
				`Started notifications for ${device.name} (${device.deviceId})`
			);
		} catch (error) {
			log((<Error>error).message);
		}
	};

	const getServices = async (device: BleDevice): Promise<BleService[]> => {
		log(`Getting services for ${device.name} (${device.deviceId})...`);
		try {
			const services = await BleClient.getServices(device.deviceId);
			log(`Got services for ${device.name} (${device.deviceId})`);
			return services;
		} catch (error) {
			log((<Error>error).message);
		}
	};

	const checkConnectedDevices = async (withServices: string[]) => {
		log(`Checking connected devices...`);
		try {
			const devices = await BleClient.getConnectedDevices(withServices);
			log(`Got connected devices`);
			return devices;
		} catch (error) {
			log((<Error>error).message);
		}
	};

	const disconnectFromDevice = async (device: BleDevice) => {
		log(`Disconnecting from ${device.name} (${device.deviceId})...`);
		try {
			await BleClient.disconnect(device.deviceId);
			connectedDevices.value = connectedDevices.value.filter(
				(d) => d.deviceId !== device.deviceId
			);
			log(`Disconnected from ${device.name} (${device.deviceId})`);
		} catch (error) {
			log((<Error>error).message);
		}
	};

	return {
		initialize,
		scan,
		connectToDevice,
		read,
		startNotifications,
		getServices,
		checkConnectedDevices,
		disconnectFromDevice,
	};
}
