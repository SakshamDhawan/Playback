import { Canvas } from "../Canvas";
import CanvasObject from "../interfaces/CanvasObject";
import CanvasBounds from "../interfaces/CanvasBounds";
import Position from "../interfaces/Position";
import CanvasOffset from "../interfaces/CanvasOffset";

const spaceBetweenChartHeadingAndChart = 10;

function convertRange( value: number, r1: [range1Min: number, range1Max: number], r2: [range2Min: number, range2Max: number] ) {
    return ( value - r1[ 0 ] ) * ( r2[ 1 ] - r2[ 0 ] ) / ( r1[ 1 ] - r1[ 0 ] ) + r2[ 0 ];
}

export class HistoryLineChart implements CanvasObject {
	title: string;
	bounds: CanvasBounds;
	value: number;
	min: number;
	max: number;
	showDebugOutline: boolean = false;
	show: boolean = true;
	history: number[] = [];
	historyIX: number = 0;
	historyMax: number = 400;
	constructor(title: string, bounds: CanvasBounds, min: number, max: number) {
		this.title = title;
		this.bounds = bounds;
		this.value = 0;
		this.min = min;
		this.max = max;
	}
	setValue(value: number) {
		this.value = value;
		this.history[this.historyIX++] = value;
        if(this.historyIX >= this.historyMax){
            this.historyIX = 0;
        }
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

		const textHeight = 16;
		const chartYStart = this.bounds.y;
		const chartDrawHeight = height - textHeight - spaceBetweenChartHeadingAndChart;
		//const chartDrawHeight = height;

		if(this.showDebugOutline) {
			canvas.ctx.strokeStyle = 'red';
			canvas.ctx.strokeRect(canvas.so(this.bounds.x, offset?.x), canvas.so(this.bounds.y, offset?.y), canvas.s(width), canvas.s(height));
		}



		// Draw the box
		canvas.ctx.strokeStyle = '#000';
		canvas.ctx.fillStyle = '#FFF';
		canvas.ctx.strokeRect(
			canvas.so(this.bounds.x, offset?.x),
			canvas.so(chartYStart, offset?.y),
			canvas.s(width),
			canvas.s(chartDrawHeight)
		);

		canvas.ctx.strokeStyle = '#00D5CA';
		canvas.ctx.lineWidth = 3;

		let lineXPosition = width;
        let ix = this.historyIX - 1;
		let i = 0;
        while(ix != this.historyIX && lineXPosition > 0){
            lineXPosition -= 1;
            if(ix < 0){
                ix += this.historyMax;
            }
			const scaledValue = convertRange(this.history[ix], [this.min, this.max], [0, chartDrawHeight]);

            let v = scaledValue;
			if(i === 0) {
				canvas.ctx.moveTo(x + canvas.s(lineXPosition), y + canvas.s(chartDrawHeight - v));
			}
            canvas.ctx.lineTo(x + canvas.s(lineXPosition), y + canvas.s(chartDrawHeight - v));
            --ix;
			i += 1;
        }

		canvas.ctx.stroke();

		canvas.ctx.fillStyle = '#000';
		canvas.ctx.strokeStyle = '#000';
		canvas.ctx.font = `${canvas.s(16)}px Arial`;
		canvas.ctx.textAlign = 'left';
		canvas.ctx.fillText(this.title, canvas.so(this.bounds.x, offset?.x), canvas.so(this.bounds.y + chartDrawHeight + spaceBetweenChartHeadingAndChart + 16, offset?.y));
		canvas.ctx.textAlign = 'right';
		canvas.ctx.fillText(this.value.toFixed(2).toString(), canvas.so(width, offset?.x), canvas.so(this.bounds.y + chartDrawHeight + spaceBetweenChartHeadingAndChart + 16, offset?.y));

		canvas.ctx.restore();

	}
}
