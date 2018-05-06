import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ShellComponent } from './shell/shell.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { MatButtonModule, MatCardModule, MatMenuModule, MatToolbarModule, MatIconModule,MatSidenavModule } from '@angular/material';
import { RouterModule, Routes } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [
                  ShellComponent
                ],
  imports: [
              BrowserModule,
              BrowserAnimationsModule,
              MatButtonModule,
              MatMenuModule,
              MatCardModule,
              MatToolbarModule,
              MatIconModule,
              MatSidenavModule,
              FlexLayoutModule,
              RouterModule.forRoot([],{enableTracing:true})
           ],
  providers: [],
  bootstrap: [ShellComponent],
  exports: [
    ShellComponent
  ]
})
export class HostModule 
{ 
    
}