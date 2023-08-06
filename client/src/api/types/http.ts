/* eslint-disable @typescript-eslint/no-explicit-any */
import { type AxiosRequestConfig } from 'axios';

export interface IHttp {
  request<T = any, R = T>(config: AxiosRequestConfig): Promise<R>;
  get<T = any, R = T>(url: string, config?: AxiosRequestConfig): Promise<R>;
  post<T = any, R = T>(
    url: string,
    data?: T,
    config?: AxiosRequestConfig
  ): Promise<R>;
  put<T = any, R = T>(
    url: string,
    data?: T,
    config?: AxiosRequestConfig
  ): Promise<R>;
  patch<T = any, R = T>(
    url: string,
    data?: T,
    config?: AxiosRequestConfig
  ): Promise<R>;
  delete<T = any, R = T>(url: string, config?: AxiosRequestConfig): Promise<R>;
}