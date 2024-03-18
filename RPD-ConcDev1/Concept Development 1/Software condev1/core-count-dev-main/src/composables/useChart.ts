
const spaceBetweenChartHeadingAndChart = 10;

export function useChart() {

	const drawRangeBar = (
		ctx: CanvasRenderingContext2D,
		scale: number,
		xStart: number,
		yStart: number,
		width: number,
		height: number,
		title: string,
		value: number,
	) => {
		const s = (n: number) => n * scale;
		//const yStart = (height * chartNumber) + (chartSpacing * (chartNumber + 1));
		const scaledVal = value * (((width)/2)/32800);

		// ctx.strokeStyle = 'red';
		// ctx.strokeRect(s(xStart), s(yStart), s(width), s(height));

		ctx.fillStyle = '#000';
		ctx.strokeStyle = '#000';
		ctx.font = `${s(16)}px Arial`;
		ctx.textAlign = 'left';
		ctx.fillText(title, s(xStart), s(yStart + 16));
		ctx.textAlign = 'right';
		ctx.fillText(value.toString(), s(width), s(yStart + 16));

		const textHeight = 16;
		const chartYStart = yStart + textHeight + spaceBetweenChartHeadingAndChart;
		const chartDrawHeight = height - textHeight - spaceBetweenChartHeadingAndChart;

		const lineWidth = 1;
		const chartCenter = width / 2;
		const lineXPosition = chartCenter + scaledVal;

		ctx.strokeStyle = '#000';
		ctx.strokeRect(s(xStart), s(chartYStart), s(width), s(chartDrawHeight));

		ctx.fillStyle = '#CCC';
		ctx.fillRect(s(width / 2), s(chartYStart - 8), s(1), s(chartDrawHeight + 16));

		ctx.fillStyle = '#000';
		ctx.fillRect(s(lineXPosition), s(chartYStart), s(lineWidth), s(chartDrawHeight));


	};

	const drawBar = (
		ctx: CanvasRenderingContext2D,
		scale: number,
		xStart: number,
		yStart: number,
		width: number,
		height: number,
		title: string,
		value: number,
	) => {
		const s = (n: number) => n * scale;
		const min = 0;
		const max = 50;

		// ctx.strokeStyle = 'red';
		// ctx.strokeRect(s(xStart), s(yStart), s(width), s(height));

		ctx.fillStyle = '#000';
		ctx.strokeStyle = '#000';
		ctx.font = `${s(16)}px Arial`;
		ctx.textAlign = 'left';
		ctx.fillText(title, s(xStart), s(yStart + 16));
		ctx.textAlign = 'right';
		ctx.fillText(value.toString(), s(width), s(yStart + 16));

		const textHeight = 16;
		const chartYStart = yStart + textHeight + spaceBetweenChartHeadingAndChart;
		const chartDrawHeight = height - textHeight - spaceBetweenChartHeadingAndChart;

		const lineWidth = 1;
		const lineXPosition = ((value - min) / (max - min) * width) + xStart;

		ctx.strokeStyle = '#000';
		ctx.strokeRect(s(xStart), s(chartYStart), s(width), s(chartDrawHeight));

		ctx.fillStyle = '#000';
		ctx.fillRect(s(lineXPosition), s(chartYStart), s(lineWidth), s(chartDrawHeight));

	}


	const drawText = (
		ctx: CanvasRenderingContext2D,
		scale: number,
		xStart: number,
		yStart: number,
		fontSize: number,
		fontColor: string,
		text: string,
		bold: boolean = false,
	) => {
		const s = (n: number) => n * scale;
		ctx.textAlign = 'left';
		ctx.fillStyle = fontColor;
		ctx.font = `${bold ? 'bold ' : ''}${s(fontSize)}px Arial`;
		ctx.fillText(text, s(xStart), s(yStart + fontSize));
	};

	return {
		drawRangeBar,
		drawText,
		drawBar,
	};
};
