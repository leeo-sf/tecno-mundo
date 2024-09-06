import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseMadeComponent } from './purchase-made.component';

describe('PurchaseMadeComponent', () => {
  let component: PurchaseMadeComponent;
  let fixture: ComponentFixture<PurchaseMadeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PurchaseMadeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PurchaseMadeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
