import CanvasObject from "./interfaces/CanvasObject";

export class Canvas {
	el: HTMLCanvasElement;
	ctx: CanvasRenderingContext2D | null;
	width: number;
	height: number;
	objects: CanvasObject[] = [];
	scale: number;
	constructor(el: HTMLCanvasElement, width: number, height: number, scale: number = 2) {
		this.el = el;
		this.ctx = this.el.getContext('2d');
		this.width = width;
		this.height = height;
		this.scale = scale;
		this.updateElementSize();
	}
	updateElementSize() {
		this.el.width = this.s(this.width);
		this.el.height = this.s(this.height);
		this.el.style.width = `${this.width}px`;
		this.el.style.height = `${this.height}px`;
	}
	/**
	 * A shorthand for scaling a number
	 * @param n Number to scale
	 * @returns Scaled value
	 */
	s(n: number) {
		return n * this.scale;
	}
	so(n: number, offset?: number) {
		if(!offset) {
			return this.s(n);
		}
		return this.s(n + offset);
	}
	draw() {

		if(!this.ctx) {
			return;
		}

		this.ctx.clearRect(0, 0, this.s(this.width), this.s(this.height));

		this.objects.forEach((object) => {
			if(!object.show) {
				return;
			}
			object.draw(this);
		});

	}
	add(object: CanvasObject) {
		this.objects.push(object);
	}
}
