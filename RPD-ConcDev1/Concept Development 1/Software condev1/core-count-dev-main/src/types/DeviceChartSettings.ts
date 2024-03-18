type DeviceChartSettings = {
	showSensorDialCharts: boolean
	sensorDial1Min: number;
	sensorDial2Min: number;
	sensorDial3Min: number;
	sensorDial1Max: number;
	sensorDial2Max: number;
	sensorDial3Max: number;
	showSensorBarCharts: boolean;
	sensorBar1Min: number;
	sensorBar2Min: number;
	sensorBar3Min: number;
	sensorBar1Max: number;
	sensorBar2Max: number;
	sensorBar3Max: number;
	showPitchRollYawCircles: boolean;
	showPitchRollYawBars: boolean;
	imuPitchBarRange: number;
	imuRollBarRange: number;
	imuYawBarRange: number;
	showImuXYZBars: boolean;
	imuXBarRange: number;
	imuYBarRange: number;
	imuZBarRange: number;
};

export default DeviceChartSettings;
