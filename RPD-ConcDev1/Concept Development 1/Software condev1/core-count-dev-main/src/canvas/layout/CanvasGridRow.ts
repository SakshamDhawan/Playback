import CanvasObject from "../interfaces/CanvasObject";

export class CanvasGridRow {
	objects: CanvasObject[] = [];
	constructor(objects: CanvasObject[] = []) {
		this.objects = objects;
	}
	getHeight() {
		let height = 0;
		return Math.max(...this.objects.map((object) => {
			return object.bounds.height;
		}));
	}
}
