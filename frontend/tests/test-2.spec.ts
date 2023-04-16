import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'open drawer' }).click();
  await page.getByRole('button', { name: 'Stations' }).click();
  await page.locator('.css-1r9jet7 > .MuiButtonBase-root').click();
  await page.getByText('Count: 10').click();
  await page.getByRole('button', { name: 'Next page' }).click();
  await page.getByText('Count: 10').click();
  await page.getByPlaceholder('Search').click();
  await page.getByPlaceholder('Search').fill('Pasila');
  await page.locator('#limit').selectOption('20');
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByText('Count: 3').click();
  await page.getByPlaceholder('Search').click({
    clickCount: 3
  });
  await page.getByPlaceholder('Search').fill('');
  await page.getByText('Count: 3').click();
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByText('Count: 20').click();
  await page.getByRole('button', { name: 'Next page' }).click();
  await page.getByText('Count: 20').click();
  await page.getByRole('button', { name: 'Reset' }).click();
  await page.getByText('Count: 10').click();
  await page.getByRole('button', { name: 'Next page' }).click();
  await page.getByText('Count: 10').click();
});