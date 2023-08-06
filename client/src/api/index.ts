import { Http } from '@/api/http';
import { type ApiConfiguration, defaultApiConfiguration } from '@/api/types';

export class Api {
  private _http: Http;
  constructor(apiConfig: ApiConfiguration = defaultApiConfiguration) {
    this._http = new Http(apiConfig);
  }

  public forceRecreate(apiConfig: ApiConfiguration = defaultApiConfiguration) {
    this._http = new Http(apiConfig);
  }
}

export const api = new Api();