import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyCouponComponent } from './apply-coupon.component';

describe('ApplyCouponComponent', () => {
  let component: ApplyCouponComponent;
  let fixture: ComponentFixture<ApplyCouponComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplyCouponComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ApplyCouponComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
