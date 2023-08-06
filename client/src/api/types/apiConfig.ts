import { config } from "@/config";

export type ApiConfiguration = {
  accessToken?: string;
  baseApiUrl: string;
}

export const defaultApiConfiguration: ApiConfiguration = {
  baseApiUrl: config.apiUrl
}