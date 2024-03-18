import { defineStore } from 'pinia';
import { Preferences } from '@capacitor/preferences';


export const useSettingsStore = defineStore('settings', () => {
	const getSetting = async (key: string) => {
		const { value } = await Preferences.get({ key });
		return value;
	};

	const setSetting = async (key: string, value: any) => {
		await Preferences.set({key, value});
	};

	const removeSetting = async (key: string) => {
		await Preferences.remove({key});
	}

	return {
		getSetting,
		setSetting,
		removeSetting,
	};
});
