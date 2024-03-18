<script setup lang="ts">
import { Capacitor } from '@capacitor/core';
import { onMounted, ref, computed } from 'vue';
import { storeToRefs } from 'pinia';
import {
	IonButton,
	IonContent,
	IonHeader,
	IonItem,
	IonList,
	IonListHeader,
	IonPage,
	IonTitle,
	IonToolbar,
} from '@ionic/vue';
import DebugLog from '@/components/DebugLog.vue';
import { useLogStore } from '@/store/log';
import { Canvas } from '@/canvas/Canvas';
import { CanvasGrid } from '@/canvas/layout/CanvasGrid';
import { CanvasGridRow } from '@/canvas/layout/CanvasGridRow';
import { CanvasHeading } from '@/canvas/ui/CanvasHeading';
import { BarChart } from '@/canvas/charts/BarChart';
import { RangeBarChart } from '@/canvas/charts/RangeBarChart';
import { CircleChart } from '@/canvas/charts/CircleChart';
import { DialChart } from '@/canvas/charts/DialChart';
import { HistoryLineChart } from '@/canvas/charts/HistoryLineChart';

const { log } = useLogStore();

const canvasEl = ref(null as HTMLCanvasElement|null);


onMounted(() => {

	if(canvasEl.value) {
		const canvas = new Canvas(canvasEl.value, 300, 600);

		let demoValue0to100ChangeAmount = 0.05;
		let demoValue0to100 = 0;

		let demoValueMinus100to100ChangeAmount = 0.2;
		let demoValueMinus100to100 = -100;

		const bar1 = new BarChart('Bar Chart 1', {
			x: 0,
			y: 0,
			width: 200,
			height: 50,
		}, 0, 100);

		const range1 = new RangeBarChart('Range Chart 1', {
			x: 0,
			y: 0,
			width: 200,
			height: 50,
		}, 100);

		const circle1 = new CircleChart('Circle Chart 1', {
			x: 0,
			y: 0,
			width: 200,
			height: 100,
		}, 0, 100);

		const dial1 = new DialChart('Dial', {
			x: 0,
			y: 0,
			width: 200,
			height: 100,
		}, 0, 100);

		const history1 = new HistoryLineChart('History Chart 1', {
			x: 0,
			y: 0,
			width: 200,
			height: 100,
		}, 0, 100);

		const grid = new CanvasGrid({
			x: 0, y: 0, width: canvas.width, height: canvas.height,
		}, [
			new CanvasGridRow([
				new CanvasHeading('Heading 1', {
					x: 0,
					y: 0,
					width: 300,
					height: 16,
				}, {
					fontSize: 16,
					fontFamily: 'Arial',
					fontWeight: 'bold',
					textAlign: 'left',
				}),
			]),
			new CanvasGridRow([
				circle1,
				circle1,
				circle1,
			]),
			new CanvasGridRow([
				dial1,
				dial1,
				dial1,
			]),
			new CanvasGridRow([
				bar1,
				bar1,
			]),
			new CanvasGridRow([
				bar1,
			]),
			new CanvasGridRow([
				range1,
			]),
			new CanvasGridRow([
				history1,
			]),
		]);

		canvas.add(grid);

		bar1.setValue(2);
		range1.setValue(-5);
		circle1.setValue(5);

		console.log(grid.getContentHeight());
		canvas.height = grid.getContentHeight();
		canvas.updateElementSize();

		//canvas.add(bar1);

		function draw() {
			canvas.draw();
			window.requestAnimationFrame(() => {

				demoValue0to100 += demoValue0to100ChangeAmount;
				if(demoValue0to100 > 100){
					demoValue0to100 = 100;
					demoValue0to100ChangeAmount *= -1;
				}else if(demoValue0to100 < 0){
					demoValue0to100 = history1.min;
					demoValue0to100ChangeAmount *= -1;
				}

				demoValueMinus100to100 += demoValueMinus100to100ChangeAmount;
				if(demoValueMinus100to100 > 100){
					demoValueMinus100to100 = 100;
					demoValueMinus100to100ChangeAmount *= -1;
				}else if(demoValueMinus100to100 < -100){
					demoValueMinus100to100 = -100;
					demoValueMinus100to100ChangeAmount *= -1;
				}


				circle1.setValue(demoValue0to100);
				history1.setValue(demoValue0to100);
				dial1.setValue(demoValue0to100);
				bar1.setValue(demoValue0to100);
				range1.setValue(demoValueMinus100to100);

				draw();
			});
		}
		draw();

	}


});


</script>

<template>
	<IonPage>
		<IonHeader>
			<IonToolbar>
				<IonTitle>Core Count Dev</IonTitle>
			</IonToolbar>
		</IonHeader>

		<IonContent :fullscreen="true">
			<div class="flex flex-col h-full overflow-hidden">
				<div class="flex-1 overflow-auto">

					<div class="ion-padding">

						<canvas ref="canvasEl"></canvas>

					</div>

				</div>
				<DebugLog class="flex-none" />
			</div>
		</IonContent>
	</IonPage>
</template>
