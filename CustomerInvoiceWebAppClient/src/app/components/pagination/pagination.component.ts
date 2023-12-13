import { Component, EventEmitter, Input, Output, OnChanges } from '@angular/core';
import { AppConfig } from '../../app.config';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnChanges {
  @Input() totalItems = 0;
  @Input() itemsPerPage = AppConfig.defaultPageSize;
  @Input() currentPage = 1;
  @Output() pageChanged = new EventEmitter<number>();

  totalPages = 0;

  ngOnChanges(): void {
    this.totalPages = Math.ceil(this.totalItems / this.itemsPerPage);
  }

  changePage(newPage: number): void {
    if (newPage >= 1 && newPage <= this.totalPages) {
      this.currentPage = newPage;
      this.pageChanged.emit(this.currentPage);
    }
  }
}
