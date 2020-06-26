import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  value : any;

  constructor( private http: HttpClient) { }

  ngOnInit() {
    this.getValue();
  }


  getValue(){
    this.http.get('https://localhost:44385/weatherforecast').subscribe(response => {
      this.value = response;
    }, error =>{
      console.log("Failed");
    })
  }
}
