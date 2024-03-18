<script setup lang="ts">
import {
	IonBackButton,
	IonButtons,
	IonButton,
	IonContent,
	IonHeader,
	IonPage,
	IonTitle,
	IonToolbar,
	IonList,
	IonListHeader,
	IonItem,
	onIonViewWillLeave,
	onIonViewDidEnter,
} from '@ionic/vue';
import { Storage } from '@ionic/storage';
import { BleCharacteristic, BleDevice, BleService, BleClient } from '@capacitor-community/bluetooth-le';
import { useRoute } from 'vue-router';
import { useDevicesStore } from '@/store/devices';
import { useLogStore } from '@/store/log';
import { useSettingsStore } from '@/store/settings';
import { computed, onMounted, ref, onBeforeUnmount, onBeforeMount, nextTick, watch } from 'vue';
import { useDevice } from '@/composables/useDevice';
import { useChart } from '@/composables/useChart';
import { useDemoData } from '@/composables/useDemoData';
import DebugLog from '@/components/DebugLog.vue';
import DeviceChartSettingsModal from '@/components/DeviceChartSettingsModal.vue';
import DeviceChartSettings from '@/types/DeviceChartSettings';
import { Canvas } from '@/canvas/Canvas';
import { CanvasGrid } from '@/canvas/layout/CanvasGrid';
import { CanvasGridRow } from '@/canvas/layout/CanvasGridRow';
import { CanvasHeading } from '@/canvas/ui/CanvasHeading';
import { BarChart } from '@/canvas/charts/BarChart';
import { RangeBarChart } from '@/canvas/charts/RangeBarChart';
import { CircleChart } from '@/canvas/charts/CircleChart';
import { HistoryLineChart } from '@/canvas/charts/HistoryLineChart';
import { DialChart } from '@/canvas/charts/DialChart';

const route = useRoute();
let settingsStore;
const devicesStore = useDevicesStore();
const { log } = useLogStore();
const { generateNumberRangeSlider } = useDemoData();
const selectedService = ref<BleService|null>(null);
const selectedCharacteristic = ref<BleCharacteristic|null>(null);
const settingsModalOpen = ref(false);
const settingDefaults = {
	showSensorDialCharts: true,
	sensorDial1Min: 0,
	sensorDial2Min: 0,
	sensorDial3Min: 0,
	sensorDial1Max: 50,
	sensorDial2Max: 50,
	sensorDial3Max: 50,
	showSensorBarCharts: true,
	sensorBar1Min: 0,
	sensorBar2Min: 0,
	sensorBar3Min: 0,
	sensorBar1Max: 50,
	sensorBar2Max: 50,
	sensorBar3Max: 50,
	showPitchRollYawCircles: true,
	showPitchRollYawBars: true,
	imuPitchBarRange: 32800,
	imuRollBarRange: 32800,
	imuYawBarRange: 32800,
	showImuXYZBars: true,
	imuXBarRange: 32800,
	imuYBarRange: 32800,
	imuZBarRange: 32800,
};
const chartSettings = ref(settingDefaults as DeviceChartSettings);

const device = ref(null as BleDevice|null);

const demoMMGData = generateNumberRangeSlider(0, 0, 50, 0.5);
const demoIMUData = generateNumberRangeSlider(0, -32799, 32800, 100);

const isDeviceConnected = computed(() => {
	return devicesStore.connectedDevices.find((connectedDevice) => {
		if(!device.value) {
			return false;
		}
		return connectedDevice.deviceId === device.value.deviceId;
	});
});

const services = computed(() => {
	if(!device.value) {
		return [];
	}
	return devicesStore.deviceServices[device.value.deviceId] || [];
});

const getServices = async () => {
	if(!device.value) {
		log('No device selected', 'error');
		return;
	}
	await devicesStore.getServices(device.value);
};

const connect = async () => {
	if(!device.value) {
		log('No device selected', 'error');
		return;
	}
	await devicesStore.connectToDevice(device.value);
	await startDeviceNotifications(device.value);
};

const disconnect = async () => {
	if(!device.value) {
		log('No device selected', 'error');
		return;
	}
	await stopDeviceNotifications(device.value);
	await devicesStore.disconnectFromDevice(device.value);

};

const { startDeviceNotifications, stopDeviceNotifications, data: deviceData } = useDevice();
const { drawBar, drawRangeBar, drawText } = useChart();

const barTitle = ['Pitch', 'Roll', 'Yaw', 'X', 'Y', 'Z', 'Chan A', 'Chan B', 'Chan C', 'Chan D'];
const chartHeight = ref(50);
const chartWidth = ref(380);
const chartSpacing = 20;
const spaceBetweenChartHeadingAndChart = 10;
const canvasEl = ref(null as HTMLCanvasElement|null);
const ctx = ref(null as CanvasRenderingContext2D|null);
const canvas = ref(null as Canvas|null);
const canvasWidth = ref(300);
const canvasHeight = ref(1000);
const canvasScale = ref(2);
const canvasWrapper = ref(null as HTMLDivElement|null);
const outerWrapper = ref(null as HTMLDivElement|null);
const chartElements = {};


const s = (val: number) => {
	return val * canvasScale.value;
};

function rawToDegs(r: number){
	return Math.round(((r+32768)/65536)*360*100)/100;
}

const chartPitch = new RangeBarChart('Pitch', {
	x: 0,
	y: 0,
	width: 200,
	height: 50,
});

const chartRoll = new RangeBarChart('Roll', {
	x: 0,
	y: 0,
	width: 200,
	height: 50,
});

const chartYaw = new RangeBarChart('Yaw', {
	x: 0,
	y: 0,
	width: 200,
	height: 50,
});

const chartCirclePitch = new CircleChart('Pitch', {
	x: 0,
	y: 0,
	width: 200,
	height: 120,
}, 0, 360);

const chartCircleRoll = new CircleChart('Roll', {
	x: 0,
	y: 0,
	width: 200,
	height: 120,
}, 0, 360);

const chartCircleYaw = new CircleChart('Yaw', {
	x: 0,
	y: 0,
	width: 200,
	height: 120,
}, 0, 360);


const chartX = new RangeBarChart('X', {
	x: 0,
	y: 0,
	width: 200,
	height: 50,
});

const chartY = new RangeBarChart('Y', {
	x: 0,
	y: 0,
	width: 200,
	height: 50,
});

const chartZ = new RangeBarChart('Z', {
	x: 0,
	y: 0,
	width: 200,
	height: 50,
});

const chartSensor1 = new BarChart('Sensor 1', {
	x: 0,
	y: 0,
	width: 200,
	height: 50,
}, 0, 50);
const chartSensor2 = new BarChart('Sensor 2', {
	x: 0,
	y: 0,
	width: 200,
	height: 50,
}, 0, 50);
const chartSensor3 = new BarChart('Sensor 3', {
	x: 0,
	y: 0,
	width: 200,
	height: 50,
}, 0, 50);

const chartDialSensor1 = new DialChart('Sensor 1', {
	x: 0,
	y: 0,
	width: 200,
	height: 90,
}, 0, 50);
const chartDialSensor2 = new DialChart('Sensor 2', {
	x: 0,
	y: 0,
	width: 200,
	height: 90,
}, 0, 50);
const chartDialSensor3 = new DialChart('Sensor 3', {
	x: 0,
	y: 0,
	width: 200,
	height: 90,
}, 0, 50);

const chartHistorySensor1 = new HistoryLineChart('Sensor 1', {
	x: 0,
	y: 0,
	width: 200,
	height: 100,
}, 0, 10);
const chartHistorySensor2 = new HistoryLineChart('Sensor 2', {
	x: 0,
	y: 0,
	width: 200,
	height: 100,
}, 0, 10);
const chartHistorySensor3 = new HistoryLineChart('Sensor 3', {
	x: 0,
	y: 0,
	width: 200,
	height: 100,
}, 0, 10);

function drawCanvas(loop = true){
	let activeIX=deviceData.IMU.nextIX-1;
	if(activeIX<0){
		activeIX+=activeIX=deviceData.IMU.maxIX;
	}

	let demoMmgValue = 0;
	let demoImuValue = 0;

	const isDemo = (!device.value) ? true : false;

	if(isDemo) {
		demoMmgValue = demoMMGData();
		demoImuValue = demoIMUData();
	}


	// IMU
	const srcIMU=deviceData.IMU.buffer[activeIX].data;
	const imuPitch = isDemo ? demoImuValue : srcIMU[0];
	const imuRoll = isDemo ? demoImuValue : srcIMU[1];
	const imuYaw = isDemo ? demoImuValue : srcIMU[2];
	const imuX = isDemo ? demoImuValue : srcIMU[3];
	const imuY = isDemo ? demoImuValue : srcIMU[4];
	const imuZ = isDemo ? demoImuValue : srcIMU[5];

	chartPitch.setValue(imuPitch);
	chartRoll.setValue(imuRoll);
	chartYaw.setValue(imuYaw);
	chartCirclePitch.setValue(rawToDegs(imuPitch));
	chartCircleRoll.setValue(rawToDegs(imuRoll));
	chartCircleYaw.setValue(rawToDegs(imuYaw));
	chartX.setValue(imuX);
	chartY.setValue(imuY);
	chartZ.setValue(imuZ);

	// MMG
	activeIX=deviceData.MMG.nextIX-1;
	if(activeIX<0){
		activeIX+=activeIX=deviceData.MMG.maxIX;
	}

	const srcMMG=deviceData.MMG.buffer[activeIX].data;

	const mmg1 = isDemo ? demoMmgValue : srcMMG[0];
	const mmg2 = isDemo ? demoMmgValue : srcMMG[1];
	const mmg3 = isDemo ? demoMmgValue : srcMMG[2];

	chartSensor1.setValue(mmg1);
	chartSensor2.setValue(mmg2);
	chartSensor3.setValue(mmg3);
	chartDialSensor1.setValue(mmg1);
	chartDialSensor2.setValue(mmg2);
	chartDialSensor3.setValue(mmg3);

	canvas.value?.draw();

	if(loop) {
		window.requestAnimationFrame(drawCanvas.bind(null, true));
	}
}

watch(chartSettings, (value) => {
	// Store settings in storage
	settingsStore.setSetting('deviceChartSettings', JSON.stringify(value));

	// Apply settings to charts
	chartDialSensor1.show = value.showSensorDialCharts;
	chartDialSensor2.show = value.showSensorDialCharts;
	chartDialSensor3.show = value.showSensorDialCharts;
	chartDialSensor1.min = value.sensorDial1Min;
	chartDialSensor2.min = value.sensorDial2Min;
	chartDialSensor3.min = value.sensorDial3Min;
	chartDialSensor1.max = value.sensorDial1Max;
	chartDialSensor2.max = value.sensorDial2Max;
	chartDialSensor3.max = value.sensorDial3Max;

	chartSensor1.show = value.showSensorBarCharts;
	chartSensor2.show = value.showSensorBarCharts;
	chartSensor3.show = value.showSensorBarCharts;
	chartSensor1.min = value.sensorBar1Min;
	chartSensor2.min = value.sensorBar2Min;
	chartSensor3.min = value.sensorBar3Min;
	chartSensor1.max = value.sensorBar1Max;
	chartSensor2.max = value.sensorBar2Max;
	chartSensor3.max = value.sensorBar3Max;

	chartCirclePitch.show = value.showPitchRollYawCircles;
	chartCircleRoll.show = value.showPitchRollYawCircles;
	chartCircleYaw.show = value.showPitchRollYawCircles;

	chartPitch.show = value.showPitchRollYawBars;
	chartRoll.show = value.showPitchRollYawBars;
	chartYaw.show = value.showPitchRollYawBars;
	chartPitch.range = value.imuPitchBarRange;
	chartRoll.range = value.imuRollBarRange;
	chartYaw.range = value.imuYawBarRange;

	chartX.show = value.showImuXYZBars;
	chartY.show = value.showImuXYZBars;
	chartZ.show = value.showImuXYZBars;
	chartX.range = value.imuXBarRange;
	chartY.range = value.imuYBarRange;
	chartZ.range = value.imuZBarRange;

	if(canvas.value) {
		canvas.value.height = (canvas.value.objects[0].getContentHeight() || 0) + 10; // 10 for padding!
		canvas.value.updateElementSize();
	}
});


const setupCanvas = (canvas: Canvas) => {
	if(!canvas) {
		return;
	}

	const gridPadding = 2;
	const grid = new CanvasGrid({
		x: 0 + 2, y: 0 + 2, width: canvas.width - 2, height: canvas.height + 2,
	}, [
		new CanvasGridRow([
			new CanvasHeading('MMG sensors', {
				x: 0,
				y: 0,
				width: 300,
				height: 16,
			}, {
				fontSize: 20,
				fontFamily: 'Arial',
				fontWeight: 'bold',
				textAlign: 'left',
				color: '#000',
			}),
		]),
		new CanvasGridRow([
			chartDialSensor1,
			chartDialSensor2,
			chartDialSensor3,
		]),
		new CanvasGridRow([
			chartSensor1,
		]),
		new CanvasGridRow([
			chartSensor2,
		]),
		new CanvasGridRow([
			chartSensor3,
		]),
		// new CanvasGridRow([
		// 	chartHistorySensor1,
		// ]),
		// new CanvasGridRow([
		// 	chartHistorySensor2,
		// ]),
		// new CanvasGridRow([
		// 	chartHistorySensor3,
		// ]),
		new CanvasGridRow([
			new CanvasHeading('IMU', {
				x: 0,
				y: 0,
				width: 300,
				height: 16,
			}, {
				fontSize: 20,
				fontFamily: 'Arial',
				fontWeight: 'bold',
				textAlign: 'left',
				color: '#000',
			}),
		]),
		new CanvasGridRow([
			chartCirclePitch,
			chartCircleRoll,
			chartCircleYaw,
		]),
		new CanvasGridRow([
			chartPitch,
		]),
		new CanvasGridRow([
			chartRoll,
		]),
		new CanvasGridRow([
			chartYaw,
		]),
		new CanvasGridRow([
			chartX,
		]),
		new CanvasGridRow([
			chartY,
		]),
		new CanvasGridRow([
			chartZ,
		]),
	]);

	canvas.add(grid);

	canvas.height = grid.getContentHeight() + 10; // 10 for padding!
	canvas.updateElementSize();
};



onBeforeMount(async () => {
	const deviceId = route.params.id as string;
	device.value = devicesStore.deviceList.find((device) => device.deviceId === deviceId) || null;

	settingsStore = await useSettingsStore();
	const storedSettings = await settingsStore.getSetting('deviceChartSettings');
	console.log('storedSettings', storedSettings)
	chartSettings.value = storedSettings ? {...settingDefaults, ...JSON.parse(storedSettings)} : settingDefaults;
});

onMounted(async () => {


	// Get inner width of canvas wrapper without padding and use it as canvas width
	if(canvasEl.value && outerWrapper.value && canvasWrapper.value) {
		// get outer wrapper width
		const pageWidth = window.innerWidth;
		const outerWrapperRect = outerWrapper.value.getBoundingClientRect();
		const outerWrapperWidth = outerWrapperRect.width;
		// get left and right padding from canvas wrapper
		const canvasWrapperStyle = window.getComputedStyle(canvasWrapper.value);
		const paddingLeft = parseInt(canvasWrapperStyle.paddingLeft);
		const paddingRight = parseInt(canvasWrapperStyle.paddingRight);
		// subtract padding from canvas width
		canvasWidth.value = pageWidth - paddingLeft - paddingRight;
		chartWidth.value = canvasWidth.value;
		canvasHeight.value = (chartHeight.value * barTitle.length) + (chartSpacing * barTitle.length);
	canvasHeight.value = 200;
		canvas.value = new Canvas(canvasEl.value, canvasWidth.value, canvasHeight.value, canvasScale.value);
		setupCanvas(canvas.value);
		nextTick(() => {
			//canvas.value?.draw();
			drawCanvas(false);
		});
	}

	if(device.value) {
		if(isDeviceConnected.value) {
			startDeviceNotifications(device.value);
		}
	}

});

onIonViewDidEnter(() => {
	drawCanvas();
});

onIonViewWillLeave(() => {
	if(device.value) {
		stopDeviceNotifications(device.value);
	}
});
</script>

<template>
	<IonPage>
		<IonHeader>
			<IonToolbar>
				<IonButtons slot="start">
					<IonBackButton defaultHref="/"></IonBackButton>
				</IonButtons>
				<IonTitle>{{
					device ? device.name || device.deviceId : 'No device'
				}}</IonTitle>
				<IonButtons slot="primary">
					<IonButton @click="connect()" v-if="!isDeviceConnected" color="success">Connect</IonButton>
					<IonButton @click="disconnect()" v-if="isDeviceConnected" color="danger">Disconnect</IonButton>
				</IonButtons>
			</IonToolbar>
		</IonHeader>
		<IonContent :fullscreen="true">
			<div class="flex flex-col h-full overflow-hidden" ref="outerWrapper">

				<div class="flex-1 overflow-auto">

					<div class="ion-padding" ref="canvasWrapper">
						<div class="mb-8">
							<IonButton expand="block" @click="settingsModalOpen = true">Settings</IonButton>
						</div>
						<canvas ref="canvasEl"></canvas>
					</div>

				</div>

				<DebugLog class="flex-none" />
			</div>

			<DeviceChartSettingsModal v-model:open="settingsModalOpen" v-model:settings="chartSettings" :defaults="settingDefaults" />

		</IonContent>
	</IonPage>
</template>
