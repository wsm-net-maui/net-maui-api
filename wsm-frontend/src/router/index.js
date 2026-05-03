import { createRouter, createWebHistory } from 'vue-router';
import Login from '../views/Login.vue';

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: Login
  },
  {
    path: '/',
    component: () => import('../layouts/AppLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      {
        path: '',
        name: 'Dashboard',
        component: () => import('../views/Dashboard.vue'),
      },
      {
        path: 'servicos',
        name: 'Servicos',
        component: () => import('../views/Servicos.vue'),
      },
      {
        path: 'funcionarios',
        name: 'Funcionarios',
        component: () => import('../views/Funcionarios.vue'),
      },
      {
        path: 'horarios',
        name: 'Horarios',
        component: () => import('../views/Horarios.vue'),
      }
    ]
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

// Guard global para rotas privadas
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token');
  if (to.meta.requiresAuth && !token) {
    next('/login');
  } else {
    next();
  }
});

export default router;
