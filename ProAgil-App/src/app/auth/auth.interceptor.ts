import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { tap } from 'rxjs/internal/operators/tap';
import { HttpInterceptor, HttpEvent, HttpRequest, HttpHandler } from '@angular/common/http';

@Injectable({providedIn: 'root'})
export class AuthInterceptor implements HttpInterceptor {
    
    constructor(private router: Router) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if(localStorage.getItem('token') != null){
            const cloneReq = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${localStorage.getItem('token')}`) 
            });
            return next.handle(cloneReq).pipe(
                tap(
                    succ => {},
                    error => {
                        if(error.status == 401){
                            this.router.navigateByUrl('user/login');
                        }
                    }
                )
            );
          } else {
            return next.handle(req.clone());
          }
    }
}