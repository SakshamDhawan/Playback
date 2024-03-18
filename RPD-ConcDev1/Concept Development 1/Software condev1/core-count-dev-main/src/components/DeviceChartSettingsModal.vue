<script lang="ts" setup>
import DeviceChartSettings from '@/types/DeviceChartSettings';
import { IonModal, IonToolbar, IonHeader, IonButtons, IonButton, IonContent, IonToggle, IonInput, IonItem, IonList } from '@ionic/vue';
import { computed, onMounted, ref } from 'vue';
const props = defineProps<{
	open: boolean;
	settings: DeviceChartSettings;
	defaults: DeviceChartSettings;
}>();

const emit = defineEmits<{
	(e: 'update:open', value: boolean): void;
	(e: 'update:settings', value: DeviceChartSettings): void;
}>();

const isOpen = computed({
	get() {
		return props.open;
	},
	set(value: boolean) {
		emit('update:open', value);
	},
});

const localSettings = ref(props.settings);

const onWillShow = () => {
	localSettings.value = {...props.settings};
};

const saveSettings = () => {
	emit('update:settings', localSettings.value);
	isOpen.value = false;
};

const resetToDefaultValues = () => {
	localSettings.value = {...props.defaults};
};

</script>

<template>
	<IonModal :is-open="isOpen" @ionModalWillPresent="onWillShow">
		<IonHeader>
			<IonToolbar>
				<IonButtons slot="start">
					<IonButton @click="isOpen = false" color="danger">Cancel</IonButton>
				</IonButtons>
				<IonButtons slot="end">
					<IonButton color="success" @click="saveSettings">Save</IonButton>
				</IonButtons>
			</IonToolbar>
		</IonHeader>
		<IonContent>
			<div class="ion-padding">

					<div class="space-y-6">
						<div>
							<IonButton expand="block" @click="resetToDefaultValues">Reset to default values</IonButton>
						</div>
						<div>
							<h2 class="text-xl mb-4 font-bold">MMG sensors</h2>
							<div>
								<h3 class="text-lg font-medium">MMG Sensor dial charts</h3>
								<IonList class="settings-list">
									<IonItem>
										<IonToggle v-model="localSettings.showSensorDialCharts">Show MMG sensor dial charts</IonToggle>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorDial1Min" label="Sensor 1 Dial Min" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorDial1Max" label="Sensor 1 Dial Max" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorDial2Min" label="Sensor 2 Dial Min" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorDial2Max" label="Sensor 2 Dial Max" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorDial3Min" label="Sensor 3 Dial Min" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorDial3Max" label="Sensor 3 Dial Max" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
								</IonList>
							</div>
							<div>
								<h3 class="text-lg font-medium">MMG Sensor bar charts</h3>
								<IonList class="settings-list">
									<IonItem>
										<IonToggle v-model="localSettings.showSensorBarCharts">Show MMG sensor bar charts</IonToggle>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorBar1Min" label="Sensor 1 Bar Min" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorBar1Max" label="Sensor 1 Bar Max" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorBar2Min" label="Sensor 2 Bar Min" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorBar2Max" label="Sensor 2 Bar Max" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorBar3Min" label="Sensor 3 Bar Min" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
									<IonItem>
										<IonInput v-model="localSettings.sensorBar3Max" label="Sensor 3 Bar Max" label-placement="stacked" type="number" required></IonInput>
									</IonItem>
								</IonList>
							</div>
						</div>
						<div>
							<h2 class="text-xl mb-4 font-bold">IMU</h2>
							<div>

								<div>
									<h3 class="text-lg font-medium">Pitch/Roll/Yaw circles</h3>
									<IonList class="settings-list">
										<IonItem>
											<IonToggle v-model="localSettings.showPitchRollYawCircles">Show Pitch/Roll/Yaw circles</IonToggle>
										</IonItem>
									</IonList>
								</div>
								<div>
									<h3 class="text-lg font-medium">Pitch/Roll/Yaw bars</h3>
									<IonList class="settings-list">
										<IonItem>
											<IonToggle v-model="localSettings.showPitchRollYawBars">Show Pitch/Roll/Yaw bars</IonToggle>
										</IonItem>
										<IonItem>
											<IonInput v-model="localSettings.imuPitchBarRange" label="Pitch Bar Range" label-placement="stacked" type="number" required></IonInput>
										</IonItem>
										<IonItem>
											<IonInput v-model="localSettings.imuRollBarRange" label="Roll Bar Range" label-placement="stacked" type="number" required></IonInput>
										</IonItem>
										<IonItem>
											<IonInput v-model="localSettings.imuYawBarRange" label="Yaw Bar Range" label-placement="stacked" type="number" required></IonInput>
										</IonItem>
									</IonList>
								</div>
								<div>
									<h3 class="text-lg font-medium">X/Y/Z bars</h3>
									<IonList class="settings-list">
										<IonItem>
											<IonToggle v-model="localSettings.showImuXYZBars">Show X/Y/Z bars</IonToggle>
										</IonItem>
										<IonItem>
											<IonInput v-model="localSettings.imuXBarRange" label="X Bar Range" label-placement="stacked" type="number" required></IonInput>
										</IonItem>
										<IonItem>
											<IonInput v-model="localSettings.imuYBarRange" label="Y Bar Range" label-placement="stacked" type="number" required></IonInput>
										</IonItem>
										<IonItem>
											<IonInput v-model="localSettings.imuZBarRange" label="Z Bar Range" label-placement="stacked" type="number" required></IonInput>
										</IonItem>
									</IonList>
								</div>
							</div>
						</div>
					</div>

			</div>
		</IonContent>
	</IonModal>
</template>


<style lang="postcss" scoped>
.settings-list {
	--ion-item-background: transparent;
	--ion-safe-area-left: 0;
	padding-bottom: 16px;
}
</style>
