import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs/operators';
import { User } from '../models/user';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]' //directives are used in htmp i.e. *ng-For etc.
})
export class HasRoleDirective implements OnInit{
  @Input() appHasRole: string[];
  user: User;

  constructor(private viewContainterRef: ViewContainerRef, private templateRef: TemplateRef<any>, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    })
   }
  ngOnInit(): void {
    //clear view if no roles
    if(!this.user?.roles || this.user == null) {
      this.viewContainterRef.clear();
      return;
    }

    if(this.user?.roles.some(role => this.appHasRole.includes(role))) {
      this.viewContainterRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainterRef.clear();
    }
  }

}
