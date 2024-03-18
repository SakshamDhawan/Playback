import LogLine from '@/types/LogLine';
import { defineStore } from 'pinia';
import { ref } from 'vue';

const LOG_LIMIT = 100;

export const useLogStore = defineStore('log', () => {
	const logLines = ref<LogLine[]>([]);
	const showDebugLogPanel = ref(false);

	const log = (line: string, type: LogLine["type"] = 'info') => {
		logLines.value.push({
			line,
			type,
		});
		if (logLines.value.length > LOG_LIMIT) {
			logLines.value.shift();
		}
	};

	const clearLog = () => {
		logLines.value = [];
	}

	const toggleDebugPanel = () => {
		showDebugLogPanel.value = !showDebugLogPanel.value;
	};

	return { logLines, log, clearLog, showDebugLogPanel, toggleDebugPanel };
});
