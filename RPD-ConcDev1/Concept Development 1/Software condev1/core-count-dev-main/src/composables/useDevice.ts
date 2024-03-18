import { BleDevice } from "@capacitor-community/bluetooth-le";
import { useDevicesStore } from '@/store/devices';
import { useLogStore } from '@/store/log';

const DATA_SERVICE_UUID = '12345678-1234-5678-1234-56789abcdef0';
const MMG_DATA_CHARACTERISTIC_UUID = '12345678-1234-5678-1234-56789abcdef1';
const IMU_DATA_CHARACTERISTIC_UUID = '12345678-1234-5678-1234-56789abcdef2';


export function useDevice() {

	const devicesStore = useDevicesStore();
	const { log } = useLogStore();

	// Create data buffers
	const data = {
		IMU: {
			nextIX: 0,
			maxIX: 50,
			buffer: [] as { dataLen: number, data: Int16Array }[]
		},
		MMG: {
			nextIX: 0,
			maxIX: 50,
			buffer: [] as { dataLen: number, data: Int16Array }[]
		}
	};
	// Initialize buffers with empty data
	for (let n = 0; n < data.IMU.maxIX; n++) {
		data.IMU.buffer.push({ dataLen: 0, data: new Int16Array(60) });
	}
	for (let n = 0; n < data.MMG.maxIX; n++) {
		data.MMG.buffer.push({ dataLen: 0, data: new Int16Array(60) });
	}

	const handleIMUData = (d: DataView) => {
		const activeIX = data.IMU.nextIX;
		const dest = data.IMU.buffer[activeIX];
		let dLen = (d.byteLength/2);

		if(dLen>60){
		  dLen=60;
		}

		dest.dataLen = dLen;

		let ix=0;
		for(let n=0;n<dLen*2;n+=2){
		  const v=d.getUint8(n)+(d.getUint8(n+1)<<8);
		  dest.data[ix++]=v;
		}

		// Point to next buffer
		if(++data.IMU.nextIX>=data.IMU.maxIX){
		  data.IMU.nextIX=0;
		}
	  }

	  // Yeah I know we could really handle both these streams in the same function, but
	  // I kept them split up to make it easier to isolate them whilst testing... That's
	  // my excuse and I'm sticking to it!
	  const handleMMGData = (d: DataView) => {
		const activeIX = data.MMG.nextIX;
		const dest = data.MMG.buffer[activeIX];
		let dLen=(d.byteLength/2);
		if(dLen>60){
		  dLen=60;
		}
		dest.dataLen=dLen;
		let ix=0;
		for(let n=0;n<dLen*2;n+=2){
		  const v=d.getUint8(n)+(d.getUint8(n+1)<<8);
		  dest.data[ix++]=v;
		}

		// Point to next buffer
		if(++data.MMG.nextIX>=data.MMG.maxIX){
		  data.MMG.nextIX=0;
		}
	  }

	const startDeviceNotifications = async (device: BleDevice) => {
		// Find the data service
		const dataService = devicesStore.deviceServices[device.deviceId].find(
			(s) => s.uuid === DATA_SERVICE_UUID
		);

		if(!dataService) {
			log(`No data service found for ${device.name} (${device.deviceId})`, 'error');
			return;
		}

		const imuDataCharacteristic = dataService.characteristics.find(
			(c) => c.uuid === IMU_DATA_CHARACTERISTIC_UUID
		);

		if(!imuDataCharacteristic) {
			log(`No IMU data characteristic found for ${device.name} (${device.deviceId})`, 'error');
			return;
		}

		const mmgDataCharacteristic = dataService.characteristics.find(
			(c) => c.uuid === MMG_DATA_CHARACTERISTIC_UUID
		);

		if(!mmgDataCharacteristic) {
			log(`No MMG data characteristic found for ${device.name} (${device.deviceId})`, 'error');
			return;
		}

		await devicesStore.startNotifications(device, dataService, imuDataCharacteristic, handleIMUData);

		await devicesStore.startNotifications(device, dataService, mmgDataCharacteristic, handleMMGData);
	};


	const stopDeviceNotifications = async (device: BleDevice) => {
		// Find the data service
		const dataService = devicesStore.deviceServices[device.deviceId].find(
			(s) => s.uuid === DATA_SERVICE_UUID
		);

		if(!dataService) {
			log(`No data service found for ${device.name} (${device.deviceId})`, 'error');
			return;
		}

		const imuDataCharacteristic = dataService.characteristics.find(
			(c) => c.uuid === IMU_DATA_CHARACTERISTIC_UUID
		);

		if(!imuDataCharacteristic) {
			log(`No IMU data characteristic found for ${device.name} (${device.deviceId})`, 'error');
			return;
		}

		const mmgDataCharacteristic = dataService.characteristics.find(
			(c) => c.uuid === MMG_DATA_CHARACTERISTIC_UUID
		);

		if(!mmgDataCharacteristic) {
			log(`No MMG data characteristic found for ${device.name} (${device.deviceId})`, 'error');
			return;
		}

		await devicesStore.stopNotifications(device, dataService, imuDataCharacteristic);
		await devicesStore.stopNotifications(device, dataService, mmgDataCharacteristic);

	};

	return {
		startDeviceNotifications,
		stopDeviceNotifications,
		data,
	};
}
