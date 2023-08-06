import axios, { type AxiosInstance, type AxiosRequestConfig, type AxiosResponse } from 'axios';
import { type IHttp, type ApiConfiguration, defaultApiConfiguration } from '@/api/types';

export class Http implements IHttp {
  private instance: AxiosInstance | null = null;
  private token: string | undefined;

  constructor(apiConfig: ApiConfiguration = defaultApiConfiguration) {
    this.initHttp(apiConfig);
  }

  public get bearer(): string | undefined {
    return this.token;
  }

  private get http(): AxiosInstance {
    return this.instance != null ? this.instance : this.initHttp();
  }

  protected addResponseTranslator(instance: AxiosInstance): void {
    instance.interceptors.response.use(
      (resp: AxiosResponse) => resp.data.data
      ,
      (error: any) => this.handleError(error)
    );
  }

  protected initHttp(
    apiConfig: ApiConfiguration = defaultApiConfiguration
  ): AxiosInstance {
    const inst = axios.create({
      baseURL: apiConfig.baseApiUrl,
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json; charset=utf-8',
        'X-Requested-With': 'XMLHttpRequest',
        ...(apiConfig.accessToken && {
          Authorization: `Bearer ${apiConfig.accessToken}`,
        }),
      },
    });

    this.addResponseTranslator(inst);

    this.instance = inst;
    return inst;
  }

  public request<T = any, R = T>(config: AxiosRequestConfig): Promise<R> {
    return this.http.request(config);
  }

  public get<T = any, R = T>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<R> {
    return this.http.get<T, R>(url, config);
  }

  public post<T = any, R = T>(
    url: string,
    data?: T,
    config?: AxiosRequestConfig
  ): Promise<R> {
    return this.http.post<T, R>(url, data, config);
  }

  public put<T = any, R = T>(
    url: string,
    data?: T,
    config?: AxiosRequestConfig
  ): Promise<R> {
    return this.http.put<T, R>(url, data, config);
  }

  public patch<T = any, R = T>(
    url: string,
    data?: T,
    config?: AxiosRequestConfig
  ): Promise<R> {
    return this.http.patch<T, R>(url, data, config);
  }

  public delete<T = any, R = T>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<R> {
    return this.http.delete<T, R>(url, config);
  }

  private handleError(error: any) {
    if (!error?.response && error?.request) {
      return;
    }

    throw new Error();
  }
}