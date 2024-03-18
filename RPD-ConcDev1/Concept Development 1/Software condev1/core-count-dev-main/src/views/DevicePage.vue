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
} from '@ionic/vue';
import { BleCharacteristic, BleDevice, BleService, BleClient } from '@capacitor-community/bluetooth-le';
import { useRoute } from 'vue-router';
import { useDevicesStore } from '@/store/devices';
import { useLogStore } from '@/store/log';
import { computed, ref } from 'vue';
import DebugLog from '@/components/DebugLog.vue';
const route = useRoute();
const devicesStore = useDevicesStore();
const { log } = useLogStore();
const selectedService = ref<BleService|null>(null);
const selectedCharacteristic = ref<BleCharacteristic|null>(null);

const device = computed(() => {
	return devicesStore.deviceList.find((device) => {
		return device.deviceId === route.params.id;
	});
});

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
};

const disconnect = async () => {
	if(!device.value) {
		log('No device selected', 'error');
		return;
	}
	await devicesStore.disconnectFromDevice(device.value);
};

const startNotificationsForSelectedCharacteristic = async () => {
	if(!device.value) {
		log('No device selected', 'error');
		return;
	}
	if(!selectedService.value) {
		log('No service selected', 'error');
		return;
	}
	if(!selectedCharacteristic.value) {
		log('No characteristic selected', 'error');
		return;
	}
	await devicesStore.startNotifications(device.value, selectedService.value, selectedCharacteristic.value, (value) => {
		log(`Notification for ${selectedCharacteristic.value.uuid} received: ${value}`);
	});
};

const stopNotificationsForSelectedCharacteristic = async () => {
	if(!device.value) {
		log('No device selected', 'error');
		return;
	}
	if(!selectedService.value) {
		log('No service selected', 'error');
		return;
	}
	if(!selectedCharacteristic.value) {
		log('No characteristic selected', 'error');
		return;
	}
	await devicesStore.stopNotifications(device.value, selectedService.value, selectedCharacteristic.value);
};


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
			<div class="flex flex-col h-full overflow-hidden">
				<div class="flex-1 overflow-auto" v-if="selectedCharacteristic">
					<div class="ion-padding">
						<IonButton expand="block" @click="selectedCharacteristic = null">Back to characteristics</IonButton>
					</div>
					<div>Characteristic</div>
					<div>{{ selectedCharacteristic.uuid }}</div>
					<div>
						<IonButton expand="block" @click="devicesStore.readCharacteristic(device, selectedService, selectedCharacteristic)">Read</IonButton>
					</div>
					<div>
						<IonButton expand="block" @click="startNotificationsForSelectedCharacteristic()">Start notifications</IonButton>
						<IonButton expand="block" @click="stopNotificationsForSelectedCharacteristic()">Stop notifications</IonButton>
					</div>
				</div>
				<div class="flex-1 overflow-auto" v-else-if="selectedService">
					<div class="ion-padding">
						<IonButton expand="block" @click="selectedService = null">Back to services</IonButton>
					</div>
					<IonList>
						<IonListHeader>Characteristics</IonListHeader>
						<IonItem
							v-for="characteristic in selectedService.characteristics"
							:key="`serive_${selectedService.uuid}_characteristic_${characteristic.uuid}`"
							button
							:detail="true"
							@click="selectedCharacteristic = characteristic"
						>
							{{ characteristic.uuid }}
						</IonItem>
					</IonList>
				</div>
				<div class="flex-1 overflow-auto" v-else>
					<div class="ion-padding">
						<IonButton expand="block" @click="getServices()">Get services</IonButton>
					</div>
					<IonList>
						<IonListHeader>Services</IonListHeader>
						<IonItem
							v-for="service in services"
							:key="`service_${service.uuid}`"
							button
							:detail="true"
							@click="selectedService = service"
						>
							{{ service.uuid }}
						</IonItem>
					</IonList>
				</div>

				<DebugLog class="flex-none" />
			</div>
		</IonContent>
	</IonPage>
</template>
