import { Canvas } from "../Canvas";
import CanvasBounds from "./CanvasBounds";
import CanvasOffset from "./CanvasOffset";
import Position from "./Position";

export default interface CanvasObject {
	bounds: CanvasBounds;
	draw(canvas: Canvas, offset?: CanvasOffset): void;
	show: boolean;
	showDebugOutline?: boolean;
}
