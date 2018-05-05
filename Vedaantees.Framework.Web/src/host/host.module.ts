import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ShellComponent } from './shell/shell.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { MatButtonModule, MatCardModule, MatMenuModule, MatToolbarModule, MatIconModule } from '@angular/material';

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
              MatIconModule
           ],
  providers: [],
  bootstrap: [ShellComponent]
})
export class HostModule 
{ 
    
}
