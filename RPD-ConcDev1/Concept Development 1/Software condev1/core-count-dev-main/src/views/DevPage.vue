<script setup lang="ts">
import { Capacitor } from '@capacitor/core';
import { onMounted, ref } from 'vue';
import { storeToRefs } from 'pinia';
import {
	IonButton,
	IonContent,
	IonHeader,
	IonItem,
	IonList,
	IonListHeader,
	IonPage,
	IonTitle,
	IonToolbar,
} from '@ionic/vue';
import DebugLog from '@/components/DebugLog.vue';
import { useDevicesStore } from '@/store/devices';
import { useLogStore } from '@/store/log';

const devicesStore = useDevicesStore();
const { log } = useLogStore();

const scan = async () => {
	try {
		await devicesStore.scanForDevices();
	} catch (error) {
		log((<Error>error).message, 'error');
	}
};

onMounted(async () => {
	try {
		await devicesStore.initialize();
		await devicesStore.scanForDevices();
	} catch (error) {
		//log((<Error>error).message);
	}
});
</script>

<template>
	<IonPage>
		<IonHeader>
			<IonToolbar>
				<IonTitle> Core Count Dev </IonTitle>
			</IonToolbar>
		</IonHeader>

		<IonContent :fullscreen="true">
			<div class="flex flex-col h-full overflow-hidden">
				<div class="flex-1 overflow-auto">
					<div class="ion-padding">
						<IonButton expand="block" @click="scan()">Scan</IonButton>
					</div>
					<IonList>
						<IonListHeader>Found devices</IonListHeader>
						<IonItem
							v-for="device in devicesStore.deviceList"
							:key="device.deviceId"
							button
							:detail="true"
							:router-link="`/device/${device.deviceId}`"
						>
							{{ device.name }}
						</IonItem>
					</IonList>
				</div>
				<DebugLog class="flex-none" />
			</div>
		</IonContent>
	</IonPage>
</template>
