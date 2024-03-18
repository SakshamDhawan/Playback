import { Canvas } from "../Canvas";
import CanvasObject from "../interfaces/CanvasObject";
import CanvasBounds from "../interfaces/CanvasBounds";
import Position from "../interfaces/Position";
import CanvasOffset from "../interfaces/CanvasOffset";

const spaceBetweenChartHeadingAndChart = 10;

export class BarChart implements CanvasObject {
	title: string;
	bounds: CanvasBounds;
	value: number;
	min: number;
	max: number;
	showDebugOutline: boolean = false;
	show: boolean = true;
	constructor(title: string, bounds: CanvasBounds, min: number, max: number) {
		this.title = title;
		this.bounds = bounds;
		this.value = 0;
		this.min = min;
		this.max = max;
	}
	setValue(value: number) {
		this.value = value;
	}
	draw(
		canvas: Canvas,
		offset: CanvasOffset,
	) : void {
		if(!canvas.ctx) {
			return;
		}

		const width = offset?.width || this.bounds.width;
		const height = offset?.height || this.bounds.height;

		if(this.showDebugOutline) {
			canvas.ctx.strokeStyle = 'red';
			canvas.ctx.strokeRect(canvas.so(this.bounds.x, offset?.x), canvas.so(this.bounds.y, offset?.y), canvas.s(width), canvas.s(height));
		}

		canvas.ctx.fillStyle = '#000';
		canvas.ctx.strokeStyle = '#000';
		canvas.ctx.font = `${canvas.s(16)}px Arial`;
		canvas.ctx.textAlign = 'left';
		canvas.ctx.fillText(this.title, canvas.so(this.bounds.x, offset?.x), canvas.so(this.bounds.y + 16, offset?.y));
		canvas.ctx.textAlign = 'right';
		canvas.ctx.fillText(this.value.toFixed(2), canvas.so(width, offset?.x), canvas.so(this.bounds.y + 16, offset?.y));

		const textHeight = 16;
		const chartYStart = this.bounds.y + textHeight + spaceBetweenChartHeadingAndChart;
		const chartDrawHeight = height - textHeight - spaceBetweenChartHeadingAndChart;

		const lineWidth = 1;
		let lineXPosition = ((this.value - this.min) / (this.max - this.min) * width) + this.bounds.x;

		if(lineXPosition > (width + this.bounds.x)) {
			lineXPosition = width;
		}

		// Draw the bar box
		canvas.ctx.strokeStyle = '#000';
		canvas.ctx.fillStyle = '#FFF';
		canvas.ctx.strokeRect(
			canvas.so(this.bounds.x, offset?.x),
			canvas.so(chartYStart, offset?.y),
			canvas.s(width),
			canvas.s(chartDrawHeight)
		);

		// // Draw the indicator line
		// canvas.ctx.fillStyle = '#000';
		// canvas.ctx.fillRect(
		// 	canvas.so(lineXPosition, offset?.x),
		// 	canvas.so(chartYStart, offset?.y),
		// 	canvas.s(lineWidth),
		// 	canvas.s(chartDrawHeight)
		// );

		canvas.ctx.fillStyle = '#00D5CA';
		canvas.ctx.fillRect(
			canvas.so(this.bounds.x, offset?.x),
			canvas.so(chartYStart, offset?.y),
			canvas.s(lineXPosition),
			canvas.s(chartDrawHeight)
		);

	}
}
