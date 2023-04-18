import { test, expect } from '@playwright/test';

test('Test Stations list', async ({ page }) => {
  await page.goto('http://localhost:8000/');
  await page.getByRole('button', { name: 'open drawer' }).click();
  await page.getByRole('button', { name: 'Stations' }).click();
  await page.locator('.css-1r9jet7 > .MuiButtonBase-root').click();
  await page.getByText('Count: 10');
  await page.getByRole('button', { name: 'Next page' }).click();
  await page.getByText('Count: 10');
  await page.getByPlaceholder('Search').click();
  await page.getByPlaceholder('Search').fill('Pasila');
  await page.locator('#limit').selectOption('20');
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByText('Count: 3');
  await page.getByPlaceholder('Search').click({
    clickCount: 3
  });
  await page.getByPlaceholder('Search').fill('');
  await page.getByText('Count: 3');
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByText('Count: 20');
  await page.getByRole('button', { name: 'Next page' }).click();
  await page.getByText('Count: 20');
  await page.getByRole('button', { name: 'Reset' }).click();
  await page.getByText('Count: 10');
  await page.getByRole('button', { name: 'Next page' }).click();
  await page.getByText('Count: 10');
});