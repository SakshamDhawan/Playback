<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { IonButton, IonIcon } from '@ionic/vue';
import { onMounted, ref, watch, nextTick } from 'vue';
import { trash, chevronDown, chevronUp } from 'ionicons/icons';
import { useLogStore } from '@/store/log';
const logStore = useLogStore();
const { logLines, showDebugLogPanel } = storeToRefs(logStore);

const clearLog = () => {
	logStore.clearLog();
};


const lineContainer = ref(null as HTMLElement | null);

onMounted(() => {
	if (lineContainer.value) {
		lineContainer.value.scrollTop = lineContainer.value.scrollHeight;
	}
});

watch(() => {
	return logLines;
}, () => {
	nextTick(() => {
		if (lineContainer.value) {
			lineContainer.value.scrollTop = lineContainer.value.scrollHeight;
		}
	});
}, {
	deep: true,
});
</script>

<template>
	<div class="bg-black text-white font-mono">
		<div class="bg-gray-500 p-2 flex justify-between">
			<IonButton @click="logStore.toggleDebugPanel()" fill="clear" size="small" color="light">
				<IonIcon v-if="!showDebugLogPanel" slot="icon-only" size="small" :icon="chevronUp"></IonIcon>
				<IonIcon v-if="showDebugLogPanel" slot="icon-only" size="small" :icon="chevronDown"></IonIcon>
			</IonButton>
			<IonButton @click="clearLog()" fill="clear" size="small" color="light"><IonIcon slot="icon-only" size="small" :icon="trash"></IonIcon></IonButton>
		</div>
		<div class="h-48 overflow-auto" ref="lineContainer" v-show="showDebugLogPanel">
			<div class="ion-padding">
				<div
					v-for="(logLine, index) in logLines" :key="`debug_log_${index}`"
					:class="{
						'text-red-500': logLine.type === 'error',
						'text-yellow-500': logLine.type === 'warning',
					}"
				>
					- {{ logLine.line }}
				</div>
			</div>
		</div>
	</div>
</template>
