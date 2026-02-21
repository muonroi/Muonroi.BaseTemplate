import { inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export interface MUiManifest {
  schemaVersion: string;
  generatedAtUtc: string;
  userId: string;
  tenantId?: string;
  groups: MUiManifestGroup[];
}

export interface MUiManifestGroup {
  groupName: string;
  groupDisplayName: string;
  items: MUiManifestItem[];
}

export interface MUiManifestItem {
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
}

export const MHeaders = {
  Authorization: 'Authorization',
  TenantId: 'X-Tenant-Id',
  CorrelationId: 'X-Correlation-Id'
} as const;

const accessTokenResolver = (): string | null => localStorage.getItem('accessToken');
const tenantResolver = (): string | null => localStorage.getItem('tenantId');

export const mAuthInterceptor: HttpInterceptorFn = (req, next) => {
  const token = accessTokenResolver();
  if (!token) {
    return next(req);
  }

  const cloned = req.clone({
    setHeaders: {
      [MHeaders.Authorization]: `Bearer ${token}`
    }
  });

  return next(cloned);
};

export const mTenantInterceptor: HttpInterceptorFn = (req, next) => {
  const tenantId = tenantResolver();
  if (!tenantId) {
    return next(req);
  }

  const cloned = req.clone({
    setHeaders: {
      [MHeaders.TenantId]: tenantId
    }
  });

  return next(cloned);
};

export const mCorrelationInterceptor: HttpInterceptorFn = (req, next) => {
  const correlationId = globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random()}`;
  const cloned = req.clone({
    setHeaders: {
      [MHeaders.CorrelationId]: correlationId
    }
  });

  return next(cloned);
};

export const mErrorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const normalized = {
        status: error.status,
        code: error.error?.error?.code ?? 'UNKNOWN_ERROR',
        message: error.error?.error?.message ?? error.message
      };

      return throwError(() => normalized);
    })
  );
};

export class MUiManifestService {
  private readonly http = inject(HttpClient);

  load(baseApiUrl: string, userId: string) {
    const url = `${baseApiUrl}/auth/ui-manifest/${userId}`;
    return this.http.get<MUiManifest>(url);
  }

  loadCurrent(baseApiUrl: string) {
    const url = `${baseApiUrl}/auth/ui-manifest/current`;
    return this.http.get<MUiManifest>(url);
  }
}

export function mCanRender(item: MUiManifestItem): boolean {
  return item.isVisible && !item.isHidden;
}

export function mCanExecute(item: MUiManifestItem): boolean {
  return item.isEnabled;
}
