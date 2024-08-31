import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinalizeOrderComponent } from './finalize-order.component';

describe('FinalizeOrderComponent', () => {
  let component: FinalizeOrderComponent;
  let fixture: ComponentFixture<FinalizeOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FinalizeOrderComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FinalizeOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
