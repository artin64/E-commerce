import type { Product, Store, Order, User } from '../models/types';

// ── Frontend Service Layer ───────────────────────────────────────────────────
// Mirrors the backend Repository Pattern: each method corresponds to a backend
// IRepository operation (GetAll, GetById, Add, Save → POST/PUT/DELETE calls).

const BASE_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5000/api';

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  });
  if (!res.ok) throw new Error(`API error ${res.status}: ${res.statusText}`);
  return res.json() as Promise<T>;
}

// ── Product API ──────────────────────────────────────────────────────────────
export const ProductService = {
  getAll:          ()                              => request<Product[]>('/products'),
  getById:         (id: number)                   => request<Product>(`/products/${id}`),
  getByStore:      (storeId: string)              => request<Product[]>(`/products?storeId=${storeId}`),
  getByCategory:   (category: string)             => request<Product[]>(`/products?category=${category}`),
  search:          (keyword: string)              => request<Product[]>(`/products?search=${keyword}`),
  add:             (product: Omit<Product, 'id' | 'isInStock'>) =>
                     request<Product>('/products', { method: 'POST', body: JSON.stringify(product) }),
  update:          (product: Product)             =>
                     request<Product>(`/products/${product.id}`, { method: 'PUT', body: JSON.stringify(product) }),
  delete:          (id: number)                   => request<void>(`/products/${id}`, { method: 'DELETE' }),
};

// ── Store API ────────────────────────────────────────────────────────────────
export const StoreService = {
  getAll:     ()                 => request<Store[]>('/stores'),
  getById:    (id: string)       => request<Store>(`/stores/${id}`),
  getActive:  ()                 => request<Store[]>('/stores?active=true'),
  create:     (name: string, ownerId: string) =>
                request<Store>('/stores', { method: 'POST', body: JSON.stringify({ name, ownerId }) }),
  verify:     (id: string)       => request<void>(`/stores/${id}/verify`, { method: 'PATCH' }),
  deactivate: (id: string)       => request<void>(`/stores/${id}/deactivate`, { method: 'PATCH' }),
  getQrUrl:   (storeId: string)  =>
    `https://api.qrserver.com/v1/create-qr-code/?size=200x200&data=https://shopplatform.com/store/${storeId}`,
};

// ── Order API ────────────────────────────────────────────────────────────────
export const OrderService = {
  getAll:      ()                     => request<Order[]>('/orders'),
  getByBuyer:  (buyerId: string)      => request<Order[]>(`/orders?buyerId=${buyerId}`),
  getByStore:  (storeId: string)      => request<Order[]>(`/orders?storeId=${storeId}`),
  place:       (buyerId: string, storeId: string, productId: number, quantity: number) =>
                 request<Order>('/orders', { method: 'POST', body: JSON.stringify({ buyerId, storeId, productId, quantity }) }),
  updateStatus:(orderId: number, status: string) =>
                 request<void>(`/orders/${orderId}/status`, { method: 'PATCH', body: JSON.stringify({ status }) }),
};

// ── User API ─────────────────────────────────────────────────────────────────
export const UserService = {
  register: (name: string, email: string, password: string) =>
              request<User>('/users/register', { method: 'POST', body: JSON.stringify({ name, email, password }) }),
  login:    (email: string, password: string) =>
              request<User>('/users/login', { method: 'POST', body: JSON.stringify({ email, password }) }),
  getById:  (id: string) => request<User>(`/users/${id}`),
};
