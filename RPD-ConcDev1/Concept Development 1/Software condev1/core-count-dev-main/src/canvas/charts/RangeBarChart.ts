import { Canvas } from "../Canvas";
import CanvasObject from "../interfaces/CanvasObject";
import CanvasBounds from "../interfaces/CanvasBounds";
import Position from "../interfaces/Position";
import CanvasOffset from "../interfaces/CanvasOffset";

const spaceBetweenChartHeadingAndChart = 10;

function convertRange( value: number, r1: [range1Min: number, range1Max: number], r2: [range2Min: number, range2Max: number] ) {
    return ( value - r1[ 0 ] ) * ( r2[ 1 ] - r2[ 0 ] ) / ( r1[ 1 ] - r1[ 0 ] ) + r2[ 0 ];
}

export class RangeBarChart implements CanvasObject {
	title: string;
	bounds: CanvasBounds;
	value: number;
	range: number;
	showDebugOutline: boolean = false;
	show: boolean = true;
	constructor(title: string, bounds: CanvasBounds, range?: number) {
		this.title = title;
		this.bounds = bounds;
		this.value = 0;
		this.range = range || 32800;
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
		const chartCenter = width / 2;

		const scaledVal = convertRange(this.value, [-this.range, this.range], [0, width]);
		let lineXPosition = scaledVal;
		//console.log(this.value, [-this.range, this.range], [-chartCenter, chartCenter])

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

		const lineWidth = 2;

		if(lineXPosition > (width + this.bounds.x)) {
			lineXPosition = width;
		}

		// Draw the center line
		canvas.ctx.fillStyle = '#EEE';
		canvas.ctx.fillRect(
			canvas.so(chartCenter, offset?.x),
			canvas.so(chartYStart - 8, offset?.y),
			canvas.s(lineWidth),
			canvas.s(chartDrawHeight + 16)
		);

		// Draw the bar box
		canvas.ctx.strokeStyle = '#000';
		canvas.ctx.strokeRect(
			canvas.so(this.bounds.x, offset?.x),
			canvas.so(chartYStart, offset?.y),
			canvas.s(width),
			canvas.s(chartDrawHeight)
		);

		// Draw the indicator line
		canvas.ctx.fillStyle = '#00D5CA';
		if(lineXPosition < chartCenter) {
			canvas.ctx.fillRect(
				canvas.so(lineXPosition, offset?.x),
				canvas.so(chartYStart, offset?.y),
				canvas.s(chartCenter - lineXPosition),
				canvas.s(chartDrawHeight)
			);
		} else {
			canvas.ctx.fillRect(
				canvas.so(chartCenter, offset?.x),
				canvas.so(chartYStart, offset?.y),
				canvas.s(lineXPosition - chartCenter),
				canvas.s(chartDrawHeight)
			);
		}

		// canvas.ctx.fillRect(
		// 	canvas.so(lineXPosition, offset?.x),
		// 	canvas.so(chartYStart, offset?.y),
		// 	canvas.s(lineWidth),
		// 	canvas.s(chartDrawHeight)
		// );

	}
}
