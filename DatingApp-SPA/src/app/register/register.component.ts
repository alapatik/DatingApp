import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
model: any = {};

@Output() cancelRegister = new EventEmitter();
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }
  cancel() {
    this.cancelRegister.emit(false);
  }

  register() {
    this.authService.register(this.model);
  }
}
