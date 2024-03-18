import { createRouter, createWebHistory } from '@ionic/vue-router';
import { RouteRecordRaw } from 'vue-router';

const routes: Array<RouteRecordRaw> = [
	{
		path: '',
		redirect: '/dev-v2',
	},
	{
		'path': '/dev',
		component: () => import('../views/DevPage.vue'),
	},
	{
		'path': '/dev-v2',
		component: () => import('../views/DevPageV2.vue'),
	},
	{
		'path': '/device/:id',
		component: () => import('../views/DevicePage.vue'),
	},
	{
		'path': '/device-v2/:id',
		component: () => import('../views/DevicePageV2.vue'),
	},
	{
		'path': '/canvas-demo',
		component: () => import('../views/CanvasDemo.vue'),
	},
];

const router = createRouter({
	history: createWebHistory(import.meta.env.BASE_URL),
	routes,
});

export default router;
