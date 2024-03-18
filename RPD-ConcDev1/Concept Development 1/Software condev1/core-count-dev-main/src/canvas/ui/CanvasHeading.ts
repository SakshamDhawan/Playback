import { Canvas } from "../Canvas";
import CanvasBounds from "../interfaces/CanvasBounds";
import CanvasFontSettings from "../interfaces/CanvasFontSettings";
import CanvasObject from "../interfaces/CanvasObject";
import CanvasOffset from "../interfaces/CanvasOffset";

export class CanvasHeading implements CanvasObject {
	bounds: CanvasBounds;
	text: string;
	fontSettings: CanvasFontSettings;
	show: boolean = true;
	constructor(text: string, bounds: CanvasBounds, fontSettings: CanvasFontSettings = {
		fontFamily: 'Arial',
		fontSize: 16,
		fontWeight: 'normal',
		textAlign: 'left',
	}) {
		this.text = text;
		this.bounds = bounds;
		this.fontSettings = fontSettings;
	}
	draw(canvas: Canvas, offset?: CanvasOffset): void {
		if(!canvas.ctx) {
			return;
		}

		canvas.ctx.font = `${this.fontSettings.fontWeight} ${canvas.s(this.fontSettings.fontSize)}px ${this.fontSettings.fontFamily}`;
		canvas.ctx.textAlign = this.fontSettings.textAlign;
		canvas.ctx.fillStyle = this.fontSettings.color || '#000';
		canvas.ctx.fillText(this.text, canvas.so(this.bounds.x, offset?.x), canvas.so(this.bounds.y + this.bounds.height, offset?.y));
	}
}
