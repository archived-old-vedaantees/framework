import { Component, OnInit, Input, Attribute } from '@angular/core';

@Component({
                 selector: 'shell',
              templateUrl: './shell.component.html',
                styleUrls: ['./shell.component.scss']
          })
export class ShellComponent implements OnInit {

  public title:string

  constructor() 
  { 

  }

  ngOnInit() 
  {
      this.checkIfThereIsOnlyOneShellInWholePage();
      let attrs = document.getElementsByTagName("shell")[0].attributes;
      Object.keys(attrs).forEach(key => { this[attrs[key].name] = attrs[key].value; });
  }

  private checkIfThereIsOnlyOneShellInWholePage():void
  {
      if(document.getElementsByTagName("shell").length!=1)
        throw "Only one shell is allowed per page.";
  }
}
