import { Component, OnInit, Input, Output,  EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  //note we are using @input() decorator to output value from parent component which is "home component "to child component which is register component 
//  @Input() valuesFromHome:any;


 //note we are using @output() decorator as an even emitter from child component "register" to parent component "home component"
 @Output() cancelRegister = new EventEmitter();
  model: any ={};

  constructor(private  authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register(){
    this.authService.register(this.model).subscribe(()=>{
      this.alertify.success('registration successful');

    }, error => {
      this.alertify.error(error);
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
    this.alertify.error("cancelled");
  }

}
