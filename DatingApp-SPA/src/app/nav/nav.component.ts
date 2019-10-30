import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
model: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }
  login(){
    this.authService.login(this.model).subscribe((response: any) =>{
      console.log('Successfully logged in.');
      }, (error: any) => {
        console.log('Failed to login. Error: ' + error);
      });
  }
  loggedIn(){
    let localToken = localStorage.getItem('token');
    return !!localToken;
  }
  logout(){
    localStorage.removeItem('token');
    console.log('Loggedout!');
  }
}
