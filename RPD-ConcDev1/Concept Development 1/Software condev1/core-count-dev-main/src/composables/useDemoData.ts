export function useDemoData() {

	const generateNumberRangeSlider = (initialValue: number, min: number, max: number, increment: number) => {
		let value = initialValue;
		let valueIncrement = increment;

		return () => {
			value += valueIncrement;
			if(value > max) {
				value = max;
				valueIncrement *= -1;
			} else if(value < min) {
				value = min;
				valueIncrement *= -1;
			}
			return value;
		}
	}

	return { generateNumberRangeSlider };

}
