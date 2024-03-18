import { Canvas } from "../Canvas";
import CanvasObject from "../interfaces/CanvasObject";
import CanvasBounds from "../interfaces/CanvasBounds";
import Position from "../interfaces/Position";
import CanvasOffset from "../interfaces/CanvasOffset";

const spaceBetweenChartHeadingAndChart = 25;

function convertRange( value: number, r1: [range1Min: number, range1Max: number], r2: [range2Min: number, range2Max: number] ) {
    return ( value - r1[ 0 ] ) * ( r2[ 1 ] - r2[ 0 ] ) / ( r1[ 1 ] - r1[ 0 ] ) + r2[ 0 ];
}

export class DialChart implements CanvasObject {
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

		canvas.ctx.save();

		const width = offset?.width || this.bounds.width;
		const height = offset?.height || this.bounds.height;
		const x = canvas.so(this.bounds.x, offset?.x);
		const y = canvas.so(this.bounds.y, offset?.y);



		const indicatorWidth=5; // degrees
		const scaledIWidth2=((Math.PI*2)/360)*indicatorWidth/2;

		const textHeight = 16;

		const outerRadius = width;
		const innerRadius = outerRadius / 2;
		const chartDrawHeight = outerRadius - spaceBetweenChartHeadingAndChart - textHeight;
		const centerX = x + canvas.s(outerRadius / 2);
		const centerY = y + canvas.s(outerRadius / 2);

		// Scale value to a ratio of the min max range which will be used to calculate the angle,
		// this will be a number between 0 and 1
		const scaledVal = convertRange(this.value, [this.min, this.max], [0, 1]);
		// Convert the ratio to degrees, we use 180 because that is the full range of the dial
		// and subtract 90 so the starting point is on the left
		let degrees = (scaledVal * 180) - 90;
		// Clamp the degrees to the min and max of the dial, subtracting the indicator width
		const minDegrees = -90 + indicatorWidth;
		const maxDegrees = 90 - indicatorWidth;
		if(degrees < minDegrees) {
			degrees = minDegrees;
		}
		if(degrees > maxDegrees) {
			degrees = maxDegrees;
		}

		const radians = ((degrees/180)*Math.PI)-(Math.PI/2);

		if(this.showDebugOutline) {
			canvas.ctx.strokeStyle = 'red';
			canvas.ctx.strokeRect(canvas.so(this.bounds.x, offset?.x), canvas.so(this.bounds.y, offset?.y), canvas.s(width), canvas.s(height));
		}

		canvas.ctx.fillStyle = '#000000';
		canvas.ctx.strokeStyle = '#000000';

		canvas.ctx.beginPath();
		canvas.ctx.arc(centerX, centerY, outerRadius, Math.PI, 0, false);
        canvas.ctx.lineTo(centerX+outerRadius-(outerRadius-innerRadius), centerY);
        canvas.ctx.arc(centerX, centerY, innerRadius, 0, Math.PI, true);
        canvas.ctx.lineTo(centerX-(innerRadius+(outerRadius-innerRadius)), centerY);
        canvas.ctx.stroke();



		// Draw indicator
		canvas.ctx.fillStyle = '#00D5CA';
		canvas.ctx.strokeStyle = '#00D5CA';
		canvas.ctx.lineWidth=(outerRadius-innerRadius)-8;
		canvas.ctx.beginPath();
        canvas.ctx.arc(centerX, centerY, innerRadius + ((outerRadius-innerRadius) / 2), radians-scaledIWidth2, radians+scaledIWidth2, false);
        canvas.ctx.fill();
        canvas.ctx.stroke();

		canvas.ctx.fillStyle = '#000';
		canvas.ctx.strokeStyle = '#000';
		canvas.ctx.font = `${canvas.s(16)}px Arial`;
		canvas.ctx.textAlign = 'center';
		canvas.ctx.fillText(this.title, centerX, centerY + spaceBetweenChartHeadingAndChart + 16);

		canvas.ctx.textAlign = 'center';
		canvas.ctx.fillText(this.value.toFixed(2), centerX, centerY );



		canvas.ctx.restore();

	}
}
