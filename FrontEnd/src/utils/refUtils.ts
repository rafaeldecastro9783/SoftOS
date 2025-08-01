import { customRef } from 'vue';

export function localRef<T = undefined>(chave: string, valor?: T) {
  return customRef<T>((track, trigger) => ({
    get: () => {
      track();
      const val = localStorage.getItem(chave);
      return val ? JSON.parse(val) : valor;
    },
    set: (val) => {
      if (val === undefined) localStorage.removeItem(chave);
      else localStorage.setItem(chave, JSON.stringify(val));
      trigger();
    },
  }));
}

export function sessionRef<T = undefined>(chave: string, valor?: T) {
  return customRef<T>((track, trigger) => ({
    get: () => {
      track();
      const val = sessionStorage.getItem(chave);
      return val ? JSON.parse(val) : valor;
    },
    set: (val) => {
      if (val === undefined) sessionStorage.removeItem(chave);
      else sessionStorage.setItem(chave, JSON.stringify(val));
      trigger();
    },
  }));
}
