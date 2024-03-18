import { ref } from "vue";

const MAX_LOG_LINES = 100;

export default function useLog() {
	const logLines = ref<string[]>([]);
	const log = (line: string) => {
		logLines.value.push(line);
		if (logLines.value.length > MAX_LOG_LINES) {
			logLines.value.shift();
		}
	};
	const clearLog = () => {
		logLines.value = [];
	};
	return {
		logLines,
		log,
		clearLog,
	};
}
