// ── Frontend Type Definitions (mirrors backend Models) ──────────────────────
// These interfaces correspond 1:1 with backend C# model classes.
// Used for type-safe API communication and UI rendering.

export interface Product {
  id: number;
  name: string;
  price: number;
  stock: number;
  category: string;
  storeId: string;
  isInStock: boolean;
}

export interface Store {
  storeId: string;
  name: string;
  ownerId: string;
  isVerified: boolean;
  isActive: boolean;
}

export interface Order {
  orderId: number;
  buyerId: string;
  storeId: string;
  productId: number;
  quantity: number;
  totalPrice: number;
  status: OrderStatus;
  createdAt: string;
}

export type OrderStatus =
  | 'Pending'
  | 'Confirmed'
  | 'Shipped'
  | 'Delivered'
  | 'Cancelled';

export interface User {
  userId: string;
  name: string;
  email: string;
  role: UserRole;
  createdAt: string;
}

export type UserRole = 'Buyer' | 'Vendor' | 'SuperAdmin';

export interface CartItem {
  product: Product;
  quantity: number;
}
