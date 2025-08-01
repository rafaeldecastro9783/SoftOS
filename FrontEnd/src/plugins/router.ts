import { useConfigStore } from 'stores';
import { createRouter, createWebHashHistory } from 'vue-router';

const LoginView = () => import('views/LoginView.vue');
const DashboardView = () => import('views/DashboardView.vue');
const TicketsView = () => import('views/TicketsView.vue');
const OrdemDeServicoView = () => import('views/OrdemDeServicoView.vue');
const ClientesView = () => import('views/ClientesView.vue');
const ProfissionaisView = () => import('views/ProfissionaisView.vue');

export const routes = {
  login: {
    path: '/',
    name: 'Login',
    component: LoginView,
  },
  dashboard: {
    path: '/dashboard',
    name: 'dashboard',
    component: DashboardView,
  },
  tickets: {
    path: '/tickets',
    name: 'tickets',
    component: TicketsView,
  },
  ordemdeservico: {
    path: '/ordensdesevico',
    name: 'ordensdeservico',
    component: OrdemDeServicoView,
  },
  clientes: {
    path: '/clientes',
    name: 'clientes',
    component: ClientesView,
  },
  profissionais: {
    path: '/profissionais',
    name: 'profissionais',
    component: ProfissionaisView,
  },
};

const router = createRouter({
  history: createWebHashHistory(),
  routes: Object.values(routes),
});

router.beforeEach(async (to, _from, next) => {
  const configStore = useConfigStore();

  const isAuthenticated = !!configStore.token;
  const isLogin = to.path === routes.login.path;

  if (!isAuthenticated && !isLogin) return next(routes.login.path);

  if (isAuthenticated && isLogin) return next(routes.dashboard.path);

  return next();
});

router.afterEach((to) => (document.title = to.name ? to.name.toString() + ' | Soft' : 'SoftOS'));

export default router;
