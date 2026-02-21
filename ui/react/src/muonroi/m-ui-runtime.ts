export type MUiManifest = {
  schemaVersion: string;
  generatedAtUtc: string;
  userId: string;
  tenantId?: string;
  groups: MUiManifestGroup[];
};

export type MUiManifestGroup = {
  groupName: string;
  groupDisplayName: string;
  items: MUiManifestItem[];
};

export type MUiManifestItem = {
  permissionName: string;
  uiKey: string;
  parentUiKey?: string;
  type: 'menu' | 'tab' | 'action';
  displayName: string;
  icon?: string;
  description?: string;
  order: number;
  route: string;
  isPublished: boolean;
  isGranted: boolean;
  isVisible: boolean;
  isEnabled: boolean;
  isHidden: boolean;
  disabledReason?: string;
  children: MUiManifestItem[];
};

export type MApiClientOptions = {
  baseApiUrl: string;
  getAccessToken?: () => string | null;
  getTenantId?: () => string | null;
};

export function createMApiClient(options: MApiClientOptions) {
  return async function mFetch<T>(path: string, init?: RequestInit): Promise<T> {
    const headers = new Headers(init?.headers ?? {});
    const token = options.getAccessToken?.();
    const tenantId = options.getTenantId?.();

    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    if (tenantId) {
      headers.set('X-Tenant-Id', tenantId);
    }

    headers.set('X-Correlation-Id', globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random()}`);

    const response = await fetch(`${options.baseApiUrl}${path}`, {
      ...init,
      headers
    });

    if (!response.ok) {
      const payload = await response.json().catch(() => ({}));
      throw {
        status: response.status,
        code: payload?.error?.code ?? 'UNKNOWN_ERROR',
        message: payload?.error?.message ?? response.statusText
      };
    }

    return (await response.json()) as T;
  };
}

export class MUiManifestClient {
  constructor(private readonly fetcher: <T>(path: string, init?: RequestInit) => Promise<T>) {}

  load(userId: string) {
    return this.fetcher<MUiManifest>(`/auth/ui-manifest/${userId}`);
  }
}

export function mCanRender(item: MUiManifestItem): boolean {
  return item.isVisible && !item.isHidden;
}

export function mCanExecute(item: MUiManifestItem): boolean {
  return item.isEnabled;
}
