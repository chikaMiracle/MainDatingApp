import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  //we add the auth service to check if the user is logged in then redirect user if they are not logged in 

  constructor(private authService: AuthService, 
    private router: Router, 
    private alertify:AlertifyService)
    {

    }
  canActivate(): boolean{
    if(this.authService.LoggedIn()){
      return true;
    } else{
      
      this.alertify.error("You shall not pass!!!");
      this.router.navigate(['/home'])
      return false
    }

   
  }
    

  
  
}
