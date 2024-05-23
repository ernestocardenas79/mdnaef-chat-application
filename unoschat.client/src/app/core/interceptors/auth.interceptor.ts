import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthorizationInterceptor implements HttpInterceptor {
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>>
  {
    // Obtener el token de autorización del almacenamiento local o de donde lo tengas disponible
    const token = localStorage.getItem('token');

    // Clonar la solicitud y agregar el encabezado de autorización si hay un token disponible
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    // Pasar la solicitud al siguiente manipulador en la cadena de interceptores
    return next.handle(request);
  }
}
