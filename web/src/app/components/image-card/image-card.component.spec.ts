import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { ImageCardComponent } from './image-card.component';

/**
 * Mock image model to be used in the component
 */
const mockImageModel = jasmine.createSpyObj('image',[],{id:'test_id',url:'test_url',
  title: 'test_title', analysed: false, analysedId: 'test_analysedId'});

describe('ImageCardComponent', () => {
  let component: ImageCardComponent;
  let fixture: ComponentFixture<ImageCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageCardComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(ImageCardComponent);
    component = fixture.componentInstance;
    component.image = mockImageModel;
    fixture.detectChanges();
  }));

  /**
   * Ensures the component has been created successfully
   */
  it('should create', () => {
    expect(component).toBeTruthy();
  });

  /**
   * Ensures that the image will have the correct src and will therefore display the correct image
   */
  it('should show correct image', ()=>{
    const imageUrl = fixture.debugElement.nativeElement.querySelector('ion-img').src;
    expect(imageUrl).toBe(mockImageModel.url);
  });

  /**
   * Ensures that the image tittle matches the mock image model's title
   */
  it('should show correct title', ()=>{
    const cardTitle = fixture.debugElement.nativeElement.querySelector('ion-card-title').innerText.trim();
    expect(cardTitle).toBe(mockImageModel.title);
  });

  /**
   * Ensures that the image's delete button calls the correct method
   */
  it('should call onDeleteImage method', ()=>{
    spyOn(component,'onDeleteImage');
    const deleteButton = fixture.debugElement.nativeElement.querySelectorAll('ion-button')[1];
    deleteButton.click();
    expect(component.onDeleteImage).toHaveBeenCalled();
  });
});
