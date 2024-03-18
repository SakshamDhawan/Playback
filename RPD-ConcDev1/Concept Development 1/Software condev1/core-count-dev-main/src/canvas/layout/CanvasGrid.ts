import CanvasObject from "../interfaces/CanvasObject";
import CanvasGridOptions from "../interfaces/CanvasGridOptions";
import { CanvasGridRow } from "./CanvasGridRow";
import { Canvas } from "../Canvas";
import CanvasBounds from "../interfaces/CanvasBounds";

export class CanvasGrid implements CanvasObject, CanvasGridOptions {
	bounds: CanvasBounds;
	rowSpacing: number = 20;
	columnSpacing: number = 20;
	rows: CanvasGridRow[] = [];
	show: boolean = true;
	constructor(bounds: CanvasBounds,rows: CanvasGridRow[] = []) {
		this.bounds = bounds;
		this.rows = rows;
	}
	getContentHeight() {
		const visibleRows = this.rows.filter((row) => {
			return row.objects.some((object) => {
				return object.show;
			});
		});
		return visibleRows.reduce((runningHeight, row) => {
			return runningHeight + row.getHeight();
		} , 0) + (this.rowSpacing * (visibleRows.length - 1));
	}
	draw(canvas: Canvas) {
		let runningY = this.bounds.y;

		this.rows.forEach((row, i) => {
			const itemsAreVisible = row.objects.some((object) => {
				return object.show;
			});

			if(!itemsAreVisible) {
				return;
			}

			let runningX = this.bounds.x;
			const columnWidth = (
				this.bounds.width - (this.columnSpacing * (row.objects.length - 1))
			) / row.objects.length;

			row.objects.forEach((object, ii) => {
				object.draw(canvas, {
					x: runningX,
					y: runningY,
					width: columnWidth,
					height: row.getHeight(),
				});
				runningX += columnWidth + this.columnSpacing;
			});
			runningY += row.getHeight() + this.rowSpacing;
		});
	}
}
