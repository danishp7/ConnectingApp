import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptor implements HttpInterceptor {
  // any error in 400's and 500's will be catched. the interceptor knows that the error occur
  // we intercept those errors here.
  // we manipulate them and show them to user as we want in respective components

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler): Observable<HttpEvent<any>>
  {
    return next.handle(req).pipe(
      catchError(httpError => {
        // to resole 400's error
        if (httpError.status === 401) {
          return throwError(httpError.statusText);
        }

        // now to deal with 500's and model errors
        if (httpError instanceof HttpErrorResponse) {
          const applicationError = httpError.headers.get('Application-Error');

          // this will take care of 500's error
          if (applicationError) {
            return throwError(applicationError);
          }

          // now for model errors
          const serverError = httpError.error;
          let modelStateErrors = "";
          if (serverError.errors && typeof serverError.errors === 'object') {
            for (const key in serverError.errors) {
              if (serverError.errors[key]) {
                modelStateErrors += serverError.errors[key] + '\n';
              }
            }
          }

          return throwError(modelStateErrors || serverError || 'Some Server Error');
        }
      })
    );   
  }
}

// now we need to add this interceptor in errorinterceptorhandler
// also add in provider array of app module
export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
